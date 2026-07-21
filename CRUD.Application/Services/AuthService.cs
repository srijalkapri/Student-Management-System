using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Application.Responses;
using CRUD.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CRUD.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITeacherRepository teacherRepository,
            IStudentRepository studentRepository,
            IGradeRepository gradeRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto loginRequest)
        {
            var response = new ServiceResponse<LoginResponseDto>();

            var user = await _userRepository.GetByUsername(loginRequest.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                response.Success = false;
                response.Message = "Invalid username or password";
                return response;
            }

            if (user.Status != UserStatus.Approved)
            {
                response.Success = false;
                response.Message = "Your account is pending admin approval";
                return response;
            }

            response.Data = await IssueTokens(user);
            response.Message = "Login successful.";

            return response;
        }

        public async Task<ServiceResponse<LoginResponseDto>> Refresh(RefreshTokenRequestDto refreshRequest)
        {
            var response = new ServiceResponse<LoginResponseDto>();

            if (string.IsNullOrWhiteSpace(refreshRequest.RefreshToken))
            {
                response.Success = false;
                response.Message = "Refresh token is required";
                return response;
            }

            var tokenHash = HashToken(refreshRequest.RefreshToken);
            var storedToken = await _refreshTokenRepository.GetByTokenHash(tokenHash);

            if (storedToken == null || !storedToken.IsActive)
            {
                response.Success = false;
                response.Message = "Invalid or expired refresh token";
                return response;
            }

            var user = await _userRepository.GetById(storedToken.UserId);
            if (user == null || user.Status != UserStatus.Approved)
            {
                storedToken.RevokedAtUtc = DateTime.UtcNow;
                await _refreshTokenRepository.Update(storedToken);

                response.Success = false;
                response.Message = "User is not allowed to refresh token";
                return response;
            }

            var (newPlainToken, newEntity) = CreateRefreshToken(user.Id);

            storedToken.RevokedAtUtc = DateTime.UtcNow;
            storedToken.ReplacedByTokenHash = newEntity.TokenHash;
            await _refreshTokenRepository.Update(storedToken);
            await _refreshTokenRepository.Add(newEntity);

            var accessToken = await GenerateJwtToken(user);
            var accessExpiresAt = DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60"));

            response.Data = new LoginResponseDto
            {
                Token = accessToken,
                ExpiresAt = accessExpiresAt,
                RefreshToken = newPlainToken,
                RefreshTokenExpiresAt = newEntity.ExpiresAtUtc,
                User = MapUser(user)
            };
            response.Message = "Token refreshed.";

            return response;
        }

        public async Task<ServiceResponse<string>> Logout(RefreshTokenRequestDto refreshRequest)
        {
            var response = new ServiceResponse<string>();

            if (!string.IsNullOrWhiteSpace(refreshRequest.RefreshToken))
            {
                var tokenHash = HashToken(refreshRequest.RefreshToken);
                var storedToken = await _refreshTokenRepository.GetByTokenHash(tokenHash);

                if (storedToken != null && storedToken.IsActive)
                {
                    storedToken.RevokedAtUtc = DateTime.UtcNow;
                    await _refreshTokenRepository.Update(storedToken);
                }
            }

            response.Data = "Logged out";
            response.Message = "Logout successful. Please clear tokens on the client.";
            return response;
        }

        public async Task<ServiceResponse<UserDto>> GetCurrentUser(int userId)
        {
            var response = new ServiceResponse<UserDto>();
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            response.Data = MapUser(user);

            return response;
        }

        public async Task<ServiceResponse<string>> Register(RegisterRequestDto registerRequest)
        {
            var response = new ServiceResponse<string>();

            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                response.Success = false;
                response.Message = "Passwords do not match";
                return response;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            var existingUser = await _userRepository.GetByUsername(registerRequest.Username);

            if (existingUser != null)
            {
                // Rejected accounts keep the username until soft-deleted;
                // allow the same username to register again.
                if (existingUser.Status != UserStatus.Rejected)
                {
                    response.Success = false;
                    response.Message = "Username already exists";
                    return response;
                }

                existingUser.PasswordHash = hashedPassword;
                existingUser.FullName = registerRequest.FullName;
                existingUser.Email = registerRequest.Email;
                existingUser.Role = "User";
                existingUser.Status = UserStatus.Pending;
                existingUser.ApprovedAt = null;
                existingUser.ApprovedByUserId = null;
                existingUser.CreatedAt = DateTime.UtcNow;

                await _userRepository.Update(existingUser);

                response.Data = "Registration submitted. Wait for admin approval.";
                response.Message = "Success";
                return response;
            }

            var user = new User
            {
                Username = registerRequest.Username,
                PasswordHash = hashedPassword,
                FullName = registerRequest.FullName,
                Email = registerRequest.Email,
                Role = "User",
                Status = UserStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.Create(user);

            response.Data = "Registration submitted. Wait for admin approval.";
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<List<PendingUserResponseDto>>> GetPendingUsers()
        {
            var response = new ServiceResponse<List<PendingUserResponseDto>>();
            var pendingUsers = await _userRepository.GetPendingUsers();

            var dtos = pendingUsers.Select(u => new PendingUserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).ToList();

            response.Data = dtos;
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<string>> ApproveUser(int userId, ApproveUserRequestDto approveRequest, int approvedByUserId)
        {
            var response = new ServiceResponse<string>();
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            if (user.Status != UserStatus.Pending)
            {
                response.Success = false;
                response.Message = "User is not pending approval";
                return response;
            }

            if (approveRequest.Role == "Teacher")
            {
                var linkResult = await LinkOrCreateTeacherProfile(user, approveRequest);
                if (!linkResult.Success)
                {
                    return linkResult;
                }
            }
            else if (approveRequest.Role == "Student")
            {
                var linkResult = await LinkOrCreateStudentProfile(user, approveRequest);
                if (!linkResult.Success)
                {
                    return linkResult;
                }
            }

            user.Status = UserStatus.Approved;
            user.Role = approveRequest.Role;
            user.ApprovedAt = DateTime.UtcNow;
            user.ApprovedByUserId = approvedByUserId;

            await _userRepository.Update(user);

            response.Data = "User approved successfully";
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<string>> RejectUser(int userId)
        {
            var response = new ServiceResponse<string>();
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            if (user.Status != UserStatus.Pending)
            {
                response.Success = false;
                response.Message = "User is not pending approval";
                return response;
            }

            await _refreshTokenRepository.RevokeAllActiveForUser(userId);

            // Soft-delete frees the username (unique index only applies to active users).
            user.Status = UserStatus.Rejected;
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            await _userRepository.Update(user);

            response.Data = "User rejected successfully";
            response.Message = "Success";
            return response;
        }

        private async Task<ServiceResponse<string>> LinkOrCreateTeacherProfile(User user, ApproveUserRequestDto approveRequest)
        {
            var response = new ServiceResponse<string>();

            if (approveRequest.TeacherId.HasValue)
            {
                var teacher = await _teacherRepository.GetTeacherEntityById(approveRequest.TeacherId.Value);
                if (teacher == null)
                {
                    response.Success = false;
                    response.Message = "Teacher profile not found.";
                    return response;
                }

                if (teacher.UserId.HasValue && teacher.UserId != user.Id)
                {
                    response.Success = false;
                    response.Message = "Teacher profile is already linked to another user.";
                    return response;
                }

                await _teacherRepository.LinkUser(teacher.Id, user.Id);
                return response;
            }

            var phone = string.IsNullOrWhiteSpace(approveRequest.PhoneNo)
                ? $"T-{user.Id}"
                : approveRequest.PhoneNo.Trim();

            var teacherId = await _teacherRepository.CreateTeacher(new Teacher
            {
                Name = string.IsNullOrWhiteSpace(user.FullName) ? user.Username : user.FullName!,
                Email = string.IsNullOrWhiteSpace(user.Email) ? $"{user.Username}@school.local" : user.Email!,
                PhoneNo = phone,
                UserId = user.Id
            });

            if (teacherId <= 0)
            {
                response.Success = false;
                response.Message = "Failed to create teacher profile.";
            }

            return response;
        }

        private async Task<ServiceResponse<string>> LinkOrCreateStudentProfile(User user, ApproveUserRequestDto approveRequest)
        {
            var response = new ServiceResponse<string>();

            if (approveRequest.StudentId.HasValue)
            {
                var student = await _studentRepository.GetStudentEntityById(approveRequest.StudentId.Value);
                if (student == null)
                {
                    response.Success = false;
                    response.Message = "Student profile not found.";
                    return response;
                }

                if (student.UserId.HasValue && student.UserId != user.Id)
                {
                    response.Success = false;
                    response.Message = "Student profile is already linked to another user.";
                    return response;
                }

                await _studentRepository.LinkUser(student.Id, user.Id);
                return response;
            }

            if (!approveRequest.GradeId.HasValue)
            {
                response.Success = false;
                response.Message = "GradeId is required when creating a new student profile.";
                return response;
            }

            var grade = await _gradeRepository.GetGradeById(approveRequest.GradeId.Value);
            if (grade == null)
            {
                response.Success = false;
                response.Message = "Grade not found.";
                return response;
            }

            var phone = string.IsNullOrWhiteSpace(approveRequest.PhoneNo)
                ? $"S-{user.Id}"
                : approveRequest.PhoneNo.Trim();

            var studentId = await _studentRepository.CreateStudent(new Student
            {
                Name = string.IsNullOrWhiteSpace(user.FullName) ? user.Username : user.FullName!,
                Email = string.IsNullOrWhiteSpace(user.Email) ? $"{user.Username}@school.local" : user.Email!,
                PhoneNo = phone,
                GradeId = approveRequest.GradeId.Value,
                UserId = user.Id
            });

            if (studentId <= 0)
            {
                response.Success = false;
                response.Message = "Failed to create student profile.";
            }

            return response;
        }

        private async Task<LoginResponseDto> IssueTokens(User user)
        {
            var accessToken = await GenerateJwtToken(user);
            var accessExpiresAt = DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60"));

            var (refreshPlain, refreshEntity) = CreateRefreshToken(user.Id);
            await _refreshTokenRepository.Add(refreshEntity);

            return new LoginResponseDto
            {
                Token = accessToken,
                ExpiresAt = accessExpiresAt,
                RefreshToken = refreshPlain,
                RefreshTokenExpiresAt = refreshEntity.ExpiresAtUtc,
                User = MapUser(user)
            };
        }

        private (string PlainToken, RefreshToken Entity) CreateRefreshToken(int userId)
        {
            var plainToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var days = int.Parse(_configuration["Jwt:RefreshTokenExpiresInDays"] ?? "7");

            var entity = new RefreshToken
            {
                UserId = userId,
                TokenHash = HashToken(plainToken),
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(days)
            };

            return (plainToken, entity);
        }

        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

        private static UserDto MapUser(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role)
            };

            if (user.Role == "Teacher")
            {
                var teacher = await _teacherRepository.GetTeacherEntityByUserId(user.Id);
                if (teacher != null)
                {
                    claims.Add(new Claim("TeacherId", teacher.Id.ToString()));
                }
            }
            else if (user.Role == "Student")
            {
                var student = await _studentRepository.GetStudentEntityByUserId(user.Id);
                if (student != null)
                {
                    claims.Add(new Claim("StudentId", student.Id.ToString()));
                }
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

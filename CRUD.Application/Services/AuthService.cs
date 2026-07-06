using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
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

            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60"));

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role
            };

            response.Data = new LoginResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = userDto
            };
            response.Message = "Login successful.";

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

            response.Data = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role
            };

            return response;
        }

        public async Task<ServiceResponse<string>> Register(RegisterRequestDto registerRequest)
        {
            var response = new ServiceResponse<string>();
            
            // Check if passwords match
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                response.Success = false;
                response.Message = "Passwords do not match";
                return response;
            }
            
            // Check if username already exists
            var existingUser = await _userRepository.GetByUsername(registerRequest.Username);
            if (existingUser != null)
            {
                response.Success = false;
                response.Message = "Username already exists";
                return response;
            }
            
            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            
            // Create new user
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
            
            user.Status = UserStatus.Rejected;
            await _userRepository.Update(user);
            
            response.Data = "User rejected successfully";
            response.Message = "Success";
            return response;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

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

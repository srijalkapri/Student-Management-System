using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using CRUD.Responses;
using Microsoft.IdentityModel.Tokens;

namespace CRUD.Services
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

        public async Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest)
        {
            var response = new ServiceResponse<LoginResponseDto>();

            var user = await _userRepository.GetByUsernameAsync(loginRequest.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                response.Success = false;
                response.Message = "Invalid username or password";
                return response;
            }

            if (user.Role != "SuperAdmin")
            {
                response.Success = false;
                response.Message = "Unauthorized role";
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

        public async Task<ServiceResponse<UserDto>> GetCurrentUserAsync(int userId)
        {
            var response = new ServiceResponse<UserDto>();
            var user = await _userRepository.GetByIdAsync(userId);

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

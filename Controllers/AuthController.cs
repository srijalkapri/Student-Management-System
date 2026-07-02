using System.Security.Claims;
using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var response = await _authService.Login(loginRequest);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpGet("Me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new ServiceResponse<UserDto> { Success = false, Message = "User not authenticated" });
            }

            var userId = int.Parse(userIdClaim.Value);
            var response = await _authService.GetCurrentUser(userId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("Logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            return Ok(new ServiceResponse<string> { Message = "Logout successful. Please clear your token on the client." });
        }
    }
}

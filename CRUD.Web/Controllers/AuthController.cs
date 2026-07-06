using System.Security.Claims;
using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Application.Responses;
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

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            var response = await _authService.Register(registerRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("PendingUsers")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingUsers()
        {
            var response = await _authService.GetPendingUsers();
            return Ok(response);
        }

        [HttpPost("Approve/{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveUser(int userId, [FromBody] ApproveUserRequestDto approveRequest)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new ServiceResponse<string> { Success = false, Message = "User not authenticated" });
            }

            var approvedByUserId = int.Parse(userIdClaim.Value);
            var response = await _authService.ApproveUser(userId, approveRequest, approvedByUserId);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Reject/{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectUser(int userId)
        {
            var response = await _authService.RejectUser(userId);
            if (!response.Success)
            {
                return BadRequest(response);
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

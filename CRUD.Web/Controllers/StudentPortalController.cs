using System.Security.Claims;
using CRUD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentPortalController : ControllerBase
    {
        private readonly IStudentPortalService _studentPortalService;

        public StudentPortalController(IStudentPortalService studentPortalService)
        {
            _studentPortalService = studentPortalService;
        }

        [HttpGet("Overview")]
        public async Task<IActionResult> GetOverview()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _studentPortalService.GetOverview(userId.Value);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("Me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _studentPortalService.GetMyProfile(userId.Value);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("Grade")]
        public async Task<IActionResult> GetMyGrade()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _studentPortalService.GetMyGrade(userId.Value);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("Subjects")]
        public async Task<IActionResult> GetMySubjects()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _studentPortalService.GetMySubjects(userId.Value);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("Teachers")]
        public async Task<IActionResult> GetMyTeachers()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _studentPortalService.GetMyTeachers(userId.Value);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        private int? GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out var userId) ? userId : null;
        }
    }
}

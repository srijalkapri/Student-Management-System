using System.Security.Claims;
using CRUD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeacherPortalController : ControllerBase
    {
        private readonly ITeacherPortalService _teacherPortalService;

        public TeacherPortalController(ITeacherPortalService teacherPortalService)
        {
            _teacherPortalService = teacherPortalService;
        }

        [HttpGet("Overview")]
        public async Task<IActionResult> GetOverview()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _teacherPortalService.GetOverview(userId.Value);
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

            var response = await _teacherPortalService.GetMyProfile(userId.Value);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("Classes")]
        public async Task<IActionResult> GetMyClasses()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _teacherPortalService.GetMyClasses(userId.Value);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("Students")]
        public async Task<IActionResult> GetMyStudents()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _teacherPortalService.GetMyStudents(userId.Value);
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

            var response = await _teacherPortalService.GetMySubjects(userId.Value);
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

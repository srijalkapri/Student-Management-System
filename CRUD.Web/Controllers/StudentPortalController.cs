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
        private readonly IExamResultService _examResultService;

        public StudentPortalController(IStudentPortalService studentPortalService, IExamResultService examResultService)
        {
            _studentPortalService = studentPortalService;
            _examResultService = examResultService;
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

        [HttpGet("Results")]
        public async Task<IActionResult> GetMyExamResults([FromQuery] int? examScheduleId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _examResultService.GetStudentResults(userId.Value, examScheduleId);
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

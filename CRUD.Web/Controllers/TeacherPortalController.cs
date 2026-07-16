using System.Security.Claims;
using CRUD.Application.DTOs;
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
        private readonly IExamResultService _examResultService;

        public TeacherPortalController(ITeacherPortalService teacherPortalService, IExamResultService examResultService)
        {
            _teacherPortalService = teacherPortalService;
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

        [HttpGet("ExamSessions")]
        public async Task<IActionResult> GetMyExamSessions()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _examResultService.GetTeacherExamSessions(userId.Value);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("ExamResults/{examSessionId}")]
        public async Task<IActionResult> GetExamResults(int examSessionId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _examResultService.GetTeacherExamResults(userId.Value, examSessionId);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("ExamResults/SaveDraft")]
        public async Task<IActionResult> SaveExamResultsDraft([FromBody] TeacherSaveExamResultsRequestDto request)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _examResultService.SaveDraft(userId.Value, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("ExamResults/Submit")]
        public async Task<IActionResult> SubmitExamResults([FromBody] TeacherSaveExamResultsRequestDto request)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _examResultService.SubmitForApproval(userId.Value, request);
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

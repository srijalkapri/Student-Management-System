using System.Security.Claims;
using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IExamResultService _examResultService;
        private readonly IReExamService _reExamService;

        public ExamController(
            IExamService examService,
            IExamResultService examResultService,
            IReExamService reExamService)
        {
            _examService = examService;
            _examResultService = examResultService;
            _reExamService = reExamService;
        }

        [HttpGet("GetAllSchedules")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var response = await _examService.GetAllSchedules();
            return Ok(response);
        }

        [HttpGet("GetSchedulesPaged")]
        public async Task<IActionResult> GetSchedulesPaged([FromQuery] PaginationParameters parameters)
        {
            var response = await _examService.GetSchedulesPaged(parameters);
            return Ok(response);
        }

        [HttpGet("GetScheduleById")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var response = await _examService.GetScheduleById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetSchedulesByGrade")]
        public async Task<IActionResult> GetSchedulesByGrade(int gradeId)
        {
            var response = await _examService.GetSchedulesByGrade(gradeId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetSchedulesByGradePaged")]
        public async Task<IActionResult> GetSchedulesByGradePaged(int gradeId, [FromQuery] PaginationParameters parameters)
        {
            var response = await _examService.GetSchedulesByGradePaged(gradeId, parameters);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("CreateSchedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] ExamScheduleRequestDto request)
        {
            var response = await _examService.CreateSchedule(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateSchedule")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] UpdateExamScheduleRequestDto request)
        {
            var response = await _examService.UpdateSchedule(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteSchedule")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var response = await _examService.DeleteSchedule(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateSession")]
        public async Task<IActionResult> UpdateSession(int id, [FromBody] ExamSessionRequestDto request)
        {
            var response = await _examService.UpdateSession(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("AddSession")]
        public async Task<IActionResult> AddSession([FromBody] AddExamSessionRequestDto request)
        {
            var response = await _examService.AddSession(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteSession")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var response = await _examService.DeleteSession(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("BulkUpdateSessions")]
        public async Task<IActionResult> BulkUpdateSessions([FromBody] BulkUpdateSessionsRequestDto request)
        {
            var response = await _examService.BulkUpdateSessions(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("ResultApprovals/Pending")]
        public async Task<IActionResult> GetPendingResultApprovals()
        {
            var response = await _examResultService.GetPendingApprovals();
            return Ok(response);
        }

        [HttpGet("ResultApprovals/{batchId}")]
        public async Task<IActionResult> GetResultBatchReview(int batchId)
        {
            var response = await _examResultService.GetBatchReview(batchId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("ResultApprovals/{batchId}/Approve")]
        public async Task<IActionResult> ApproveResultBatch(int batchId, [FromBody] ReviewExamResultsRequestDto request)
        {
            var adminUserId = GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized();
            }

            var response = await _examResultService.ApproveBatch(batchId, adminUserId.Value, request.Comment);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("ResultApprovals/{batchId}/Reject")]
        public async Task<IActionResult> RejectResultBatch(int batchId, [FromBody] ReviewExamResultsRequestDto request)
        {
            var adminUserId = GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized();
            }

            var response = await _examResultService.RejectBatch(batchId, adminUserId.Value, request.Comment);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("Results/BySchedule/{examScheduleId}")]
        public async Task<IActionResult> GetResultsBySchedule(int examScheduleId)
        {
            var response = await _examResultService.GetMarksBySchedule(examScheduleId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("Results/ByStudent/{studentId}")]
        public async Task<IActionResult> GetResultsByStudent(int studentId, [FromQuery] int? examScheduleId = null)
        {
            var response = await _examResultService.GetMarksByStudent(studentId, examScheduleId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("ReExams/Pending")]
        public async Task<IActionResult> GetPendingReExamRequests()
        {
            var response = await _reExamService.GetPendingRequestApprovals();
            return Ok(response);
        }

        [HttpGet("ReExams/Marks/Pending")]
        public async Task<IActionResult> GetPendingReExamMarks()
        {
            var response = await _reExamService.GetPendingMarksApprovals();
            return Ok(response);
        }

        [HttpGet("ReExams/{id}")]
        public async Task<IActionResult> GetReExamById(int id)
        {
            var response = await _reExamService.GetById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("ReExams/{id}/Approve")]
        public async Task<IActionResult> ApproveReExamRequest(int id, [FromBody] ReviewReExamRequestDto request)
        {
            var adminUserId = GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized();
            }

            var response = await _reExamService.ApproveRequest(id, adminUserId.Value, request.Comment);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("ReExams/{id}/Reject")]
        public async Task<IActionResult> RejectReExamRequest(int id, [FromBody] ReviewReExamRequestDto request)
        {
            var adminUserId = GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized();
            }

            var response = await _reExamService.RejectRequest(id, adminUserId.Value, request.Comment);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("ReExams/{id}/ApproveMarks")]
        public async Task<IActionResult> ApproveReExamMarks(int id, [FromBody] ReviewReExamRequestDto request)
        {
            var adminUserId = GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized();
            }

            var response = await _reExamService.ApproveMarks(id, adminUserId.Value, request.Comment);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("ReExams/{id}/RejectMarks")]
        public async Task<IActionResult> RejectReExamMarks(int id, [FromBody] ReviewReExamRequestDto request)
        {
            var adminUserId = GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized();
            }

            var response = await _reExamService.RejectMarks(id, adminUserId.Value, request.Comment);
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

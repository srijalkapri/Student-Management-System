using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
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
    }
}

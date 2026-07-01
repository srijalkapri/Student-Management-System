using CRUD.DTOs;
using CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost("CreateSubject")]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectCreateDto subjectDto)
        {
            var response = await _subjectService.CreateSubject(subjectDto);
            return Ok(response);
        }

        [HttpGet("GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var response = await _subjectService.GetAllSubjects();
            return Ok(response);
        }

        [HttpGet("GetSubjectsPaged")]
        public async Task<IActionResult> GetSubjectsPaged([FromQuery] PaginationParameters parameters)
        {
            var response = await _subjectService.GetSubjectsPagedAsync(parameters);
            return Ok(response);
        }

        [HttpGet("GetSubjectById")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var response = await _subjectService.GetSubjectById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateSubject")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] SubjectCreateDto subjectDto)
        {
            var response = await _subjectService.UpdateSubject(id, subjectDto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteSubject")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var response = await _subjectService.DeleteSubject(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}

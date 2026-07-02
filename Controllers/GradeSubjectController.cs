using CRUD.DTOs;
using CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeSubjectController : ControllerBase
    {
        private readonly IGradeSubjectService _gradeSubjectService;

        public GradeSubjectController(IGradeSubjectService gradeSubjectService)
        {
            _gradeSubjectService = gradeSubjectService;
        }

        [HttpPost("CreateGradeSubject")]
        public async Task<IActionResult> CreateGradeSubject([FromBody] GradeSubjectCreateDto gradeSubjectDto)
        {
            var response = await _gradeSubjectService.CreateGradeSubject(gradeSubjectDto);
            return Ok(response);
        }

        [HttpGet("GetAllGradeSubjects")]
        public async Task<IActionResult> GetAllGradeSubjects(bool? isOptional = null)
        {
            var response = await _gradeSubjectService.GetAllGradeSubjects(isOptional);
            return Ok(response);
        }

        [HttpGet("GetGradeSubjectsPaged")]
        public async Task<IActionResult> GetGradeSubjectsPaged([FromQuery] PaginationParameters parameters)
        {
            var response = await _gradeSubjectService.GetGradeSubjectsPaged(parameters);
            return Ok(response);
        }

        [HttpGet("GetGradeSubjectById")]
        public async Task<IActionResult> GetGradeSubjectById(int id)
        {
            var response = await _gradeSubjectService.GetGradeSubjectById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetGradeSubjectsByGradeId")]
        public async Task<IActionResult> GetGradeSubjectsByGradeId(int gradeId, bool? isOptional = null)
        {
            var response = await _gradeSubjectService.GetGradeSubjectsByGradeId(gradeId, isOptional);
            return Ok(response);
        }

        [HttpGet("GetGradeSubjectsByGradeIdPaged")]
        public async Task<IActionResult> GetGradeSubjectsByGradeIdPaged(int gradeId, [FromQuery] PaginationParameters parameters)
        {
            var response = await _gradeSubjectService.GetGradeSubjectsByGradeIdPaged(gradeId, parameters);
            return Ok(response);
        }

        [HttpPut("UpdateGradeSubject")]
        public async Task<IActionResult> UpdateGradeSubject(int id, [FromBody] GradeSubjectCreateDto gradeSubjectDto)
        {
            var response = await _gradeSubjectService.UpdateGradeSubject(id, gradeSubjectDto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteGradeSubject")]
        public async Task<IActionResult> DeleteGradeSubject(int id)
        {
            var response = await _gradeSubjectService.DeleteGradeSubject(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}

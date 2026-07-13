using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class GradeSubjectTeacherController : ControllerBase
    {
        private readonly IGradeSubjectTeacherService _service;

        public GradeSubjectTeacherController(IGradeSubjectTeacherService service)
        {
            _service = service;
        }

        [HttpPost("CreateGradeSubjectTeacher")]
        public async Task<IActionResult> CreateGradeSubjectTeacher([FromBody] GradeSubjectTeacherCreateDto dto)
        {
            var response = await _service.CreateGradeSubjectTeacher(dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllGradeSubjectTeachers")]
        public async Task<IActionResult> GetAllGradeSubjectTeachers()
        {
            var response = await _service.GetAllGradeSubjectTeachers();
            return Ok(response);
        }

        [HttpGet("GetGradeSubjectTeachersPaged")]
        public async Task<IActionResult> GetGradeSubjectTeachersPaged([FromQuery] PaginationParameters parameters)
        {
            var response = await _service.GetGradeSubjectTeachersPaged(parameters);
            return Ok(response);
        }

        [HttpGet("GetGradeSubjectTeacherById")]
        public async Task<IActionResult> GetGradeSubjectTeacherById(int id)
        {
            var response = await _service.GetGradeSubjectTeacherById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateGradeSubjectTeacher")]
        public async Task<IActionResult> UpdateGradeSubjectTeacher(int id, [FromBody] GradeSubjectTeacherCreateDto dto)
        {
            var response = await _service.UpdateGradeSubjectTeacher(id, dto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteGradeSubjectTeacher")]
        public async Task<IActionResult> DeleteGradeSubjectTeacher(int id)
        {
            var response = await _service.DeleteGradeSubjectTeacher(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
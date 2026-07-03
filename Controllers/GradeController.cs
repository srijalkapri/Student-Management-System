using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost("CreateGrade")]
        public async Task<IActionResult> CreateGrade([FromBody] GradeCreateDto gradeDto)
        {
            var response = await _gradeService.CreateGrade(gradeDto);
            return Ok(response);
        }

        [HttpGet("GetAllGrades")]
        public async Task<IActionResult> GetAllGrades()
        {
            var response = await _gradeService.GetAllGrades();
            return Ok(response);
        }

        [HttpGet("GetGradesPaged")]
        public async Task<IActionResult> GetGradesPaged([FromQuery] PaginationParameters parameters)
        {
            var response = await _gradeService.GetGradesPaged(parameters);
            return Ok(response);
        }

        [HttpGet("GetGradeById")]
        public async Task<IActionResult> GetGradeById(int id)
        {
            var response = await _gradeService.GetGradeById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateGrade")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] GradeCreateDto gradeDto)
        { 
            var response = await _gradeService.UpdateGrade(id, gradeDto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteGrade")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var response = await _gradeService.DeleteGrade(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}

using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Teacher")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentDto)
        {
            var response = await _studentService.CreateStudent(studentDto);
            return Ok(response);
        }

        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var response = await _studentService.GetAllStudents();
            return Ok(response);
        }

        [HttpGet("GetStudentsPaged")]
        public async Task<IActionResult> GetStudentsPaged([FromQuery] PaginationParameters parameters)
        {
            var response = await _studentService.GetStudentsPaged(parameters);
            return Ok(response);
        }

        [HttpGet("GetStudentById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var response = await _studentService.GetStudentById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentCreateDto studentDto)
        {
            var response = await _studentService.UpdateStudent(id, studentDto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var response = await _studentService.DeleteStudent(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("RestoreStudent")]
        public async Task<IActionResult> RestoreStudent(int id)
        {
            var response = await _studentService.RestoreStudent(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetStudentsByGrade")]
        public async Task<IActionResult> GetStudentsByGrade(int gradeId)
        {
            var response = await _studentService.GetStudentsByGradeId(gradeId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetStudentsByGradePaged")]
        public async Task<IActionResult> GetStudentsByGradePaged(int gradeId, [FromQuery] PaginationParameters parameters)
        {
            var response = await _studentService.GetStudentsByGradeIdPaged(gradeId, parameters);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("PreviewPromotion")]
        public async Task<IActionResult> PreviewPromotion([FromBody] PromoteStudentsRequestDto request)
        {
            var response = await _studentService.PreviewPromotion(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("PromoteStudents")]
        public async Task<IActionResult> PromoteStudents([FromBody] PromoteStudentsRequestDto request)
        {
            var response = await _studentService.PromoteStudents(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}

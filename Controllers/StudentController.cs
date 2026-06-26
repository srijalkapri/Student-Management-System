using CRUD.DTOs;
using CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

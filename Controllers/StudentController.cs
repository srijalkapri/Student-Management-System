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
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreate studentdto)
        {
            var response = await _studentService.CreateStudent(studentdto);

            return Ok(response);
  }

        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()

        {

            var response = await _studentService.GetAllStudents();

            return Ok(response);

        }

        [HttpGet("GetStudentsById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var response = await _studentService.GetStudentById(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPut("UpdateStudents")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentCreate studentDto)
        {
            var response = await _studentService.UpdateStudent(id, studentDto);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteStudents")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var response = await _studentService.DeleteStudent(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }




    }

}

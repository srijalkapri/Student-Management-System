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
            return CreatedAtAction(nameof(GetStudentById), new { id = response.Data }, response);
            }

            var studentId = await _studentService.CreateStudent(studentdto);

            return CreatedAtAction(nameof(GetStudentById), new { id = studentId }, studentdto);
        }

        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {

            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("GetStudentsById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found. please enter the valid Id");
            }

            return Ok(student);
        }



        [HttpPut("UpdateStudents")]

        public async Task<IActionResult> UpdateStudent(int id ,[FromBody] StudentCreate studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultId = await _studentService.UpdateStudent(id, studentDto);

            if (resultId == 0)
            {
                return NotFound($"Student with ID {id} not found. please enter the valid Id");

            }

            return Ok(new { Message = "Student updated successfully", studentId = resultId });
        }



        [HttpDelete("DeleteStudents")]

        public async Task<IActionResult> DeleteStudent(int id)
        {
            var resultId = await _studentService.DeleteStudent(id);

            if (resultId == 0)
            {
                return NotFound($"Student with ID {id} not found. please enter the valid Id");
            }

            return Ok(new { Message = "Student deleted sucessfully", studentId = resultId });
        }



    }
}






using CRUD.DTOs;
using CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {

        private readonly ITeacherServices _teacherService;

        public TeacherController(ITeacherServices teacherService)
        {
            _teacherService = teacherService;

        }

        [HttpPost("CreateTeacher")]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherCreateDto teacherdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _teacherService.CreateTeacher(teacherdto);
            return CreatedAtAction(nameof(GetTeacherById), new { id = result }, teacherdto);
        }

        [HttpGet("GetAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeachers();
            return Ok(teachers);
        }


        [HttpGet("GetTeacherById")]
        public async Task<IActionResult> GetTeacherById(int id)
        {

            var teacherId = await _teacherService.GetTeacherById(id);

            if (teacherId == null)
            {
                return NotFound("Teacher with ID {id} not found. please enter the valid Id");
            }

            return Ok(teacherId);
        }


        [HttpGet("GetTeacherDetails")]

        public async Task<IActionResult> GetTeacherDetails(int id)
        {

            var teacherDetails = await _teacherService.GetTeacherDetails(id);

            if (teacherDetails == null)
            {
                return NotFound($"Teacher with ID {id} not found. Please enter a valid ID.");
            }

            return Ok(teacherDetails);

        }


        [HttpPut("UpdateTeacher")]

        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherCreateDto teacherdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _teacherService.UpdateTeacher(id, teacherdto);
            if (result == 0)
            {
                return NotFound($"Teacher with ID {id} not found. Please enter a valid ID.");
            }
            return NoContent();
        }

        [HttpDelete("DeleteTeacher")]

        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var result = await _teacherService.DeleteTeacher(id);
            if (result == 0)
            {
                return NotFound($"Teacher with ID {id} not found. Please enter a valid ID.");
            }
            return NoContent();

        }
}
}

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
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherCreateDto teacherDto)
        {
            var response = await _teacherService.CreateTeacher(teacherDto);
            return Ok(response);
        }

        [HttpGet("GetAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var response = await _teacherService.GetAllTeachers();
            return Ok(response);
        }

        [HttpGet("GetTeachersPaged")]
        public async Task<IActionResult> GetTeachersPaged([FromQuery] PaginationParameters parameters)
        {
            var response = await _teacherService.GetTeachersPaged(parameters);
            return Ok(response);
        }

        [HttpGet("GetTeacherById")]
        public async Task<IActionResult> GetTeacherById(int id)
        {

            var response = await _teacherService.GetTeacherById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetTeacherDetails")]
        public async Task<IActionResult> GetTeacherDetails(int id)
        {

            var response = await _teacherService.GetTeacherDetails(id);
            if (!response.Success)


            {

                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPut("UpdateTeacher")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherCreateDto teacherDto)
        {
            var response = await _teacherService.UpdateTeacher(id, teacherDto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteTeacher")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var response = await _teacherService.DeleteTeacher(id);
            if (!response.Success)
            {
                if (response.Message.Contains("class teacher"))
                    return BadRequest(response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("RestoreTeacher")]
        public async Task<IActionResult> RestoreTeacher(int id)
        {
            var response = await _teacherService.RestoreTeacher(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}

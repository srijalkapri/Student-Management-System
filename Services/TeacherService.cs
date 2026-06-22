using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using CRUD.Responses;

namespace CRUD.Services
{
    public class TeacherService : ITeacherServices
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<ServiceResponse<int>> CreateTeacher(TeacherCreateDto teacherDto)
        {
            var response = new ServiceResponse<int>();
            var teacher = new Teacher
            {
                Name = teacherDto.Name
            };

            var teacherId = await _teacherRepository.CreateTeacher(teacher);
            response.Data = teacherId;
            response.Message = "Teacher created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateTeacher(int id, TeacherCreateDto teacherDto)
        {
            var response = new ServiceResponse<int>();
            var teacher = new Teacher
            {
                Id = id,
                Name = teacherDto.Name
            };

            var result = await _teacherRepository.UpdateTeacher(teacher);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Teacher with ID {id} not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Teacher updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteTeacher(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _teacherRepository.DeleteTeacher(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Teacher with ID {id} not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Teacher deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<TeacherResponseDto>>> GetAllTeachers()
        {
            var response = new ServiceResponse<List<TeacherResponseDto>>();
            var teachers = await _teacherRepository.GetAllTeachers();
            response.Data = teachers;
            response.Message = "All teachers retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<TeacherResponseDto?>> GetTeacherById(int id)
        {
            var response = new ServiceResponse<TeacherResponseDto?>();
            var teacher = await _teacherRepository.GetTeacherById(id);
            if (teacher == null)
            {
                response.Success = false;
                response.Message = $"Teacher with ID {id} not found.";
                return response;
            }

            response.Data = teacher;
            response.Message = "Teacher retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<TeacherDetailsDto?>> GetTeacherDetails(int id)
        {
            var response = new ServiceResponse<TeacherDetailsDto?>();
            var details = await _teacherRepository.GetTeacherDetails(id);
            if (details == null)
            {
                response.Success = false;
                response.Message = $"Teacher details for ID {id} not found.";
                return response;
            }

            response.Data = details;
            response.Message = "Teacher details retrieved successfully.";
            return response;
        }
    }
}

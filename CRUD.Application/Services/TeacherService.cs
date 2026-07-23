using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using CRUD.Application.Responses;
using Microsoft.EntityFrameworkCore.Query;

namespace CRUD.Application.Services
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
                Name = teacherDto.Name,
                Email = teacherDto.Email,
                PhoneNo = teacherDto.PhoneNo
            };

            var teacherId = await _teacherRepository.CreateTeacher(teacher);
            response.Data = teacherId;
            response.Message = "Teacher created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateTeacher(int id, TeacherCreateDto teacherDto)
        {
            var response = new ServiceResponse<int>();
            var result = await _teacherRepository.UpdateTeacher(id, teacherDto.Name, teacherDto.Email, teacherDto.PhoneNo);
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

            var isClassTeacher = await _teacherRepository.IsTeacherClassTeacher(id);
            if (isClassTeacher)
            {
                response.Success = false;
                response.Message = "Cannot delete teacher assigned as class teacher. Reassign class teacher first.";
                return response;
            }

            var result = await _teacherRepository.DeleteTeacher(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Teacher with ID {id} not found.";
                return response;
            }
            if (result == -1)
            {
                response.Success = false;
                response.Message = $"Teacher with ID {id} is already deleted.";
                return response;
            }

            response.Data = result;
            response.Message = "Teacher deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> RestoreTeacher(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _teacherRepository.RestoreTeacher(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Teacher with ID {id} not found.";
                return response;
            }
            if (result == -1)
            {
                response.Success = false;
                response.Message = $"Teacher with ID {id} is not deleted.";
                return response;
            }

            response.Data = result;
            response.Message = "Teacher restored successfully.";
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

        public async Task<ServiceResponse<PagedResult<TeacherResponseDto>>> GetTeachersPaged(PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<TeacherResponseDto>>();

            var pagedResult = await _teacherRepository.GetTeachersPaged(parameters);

            response.Data = pagedResult;
            response.Message = "Teachers retrieved successfully.";
            return response;
        }
    }
}




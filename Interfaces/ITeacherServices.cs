using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface ITeacherServices
    {
        Task<ServiceResponse<int>> CreateTeacher(TeacherCreateDto teacherDto);
        Task<ServiceResponse<int>> UpdateTeacher(int id, TeacherCreateDto teacherDto);
        Task<ServiceResponse<int>> DeleteTeacher(int id);
        Task<ServiceResponse<List<TeacherResponseDto>>> GetAllTeachers();
        Task<ServiceResponse<TeacherResponseDto?>> GetTeacherById(int id);
        Task<ServiceResponse<TeacherDetailsDto?>> GetTeacherDetails(int id);
    }
}

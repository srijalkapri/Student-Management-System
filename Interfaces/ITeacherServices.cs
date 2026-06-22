using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface ITeacherServices
    {

        Task<ServiceResponse<int>> CreateTeacher(TeacherCreateDto teacherdto);
        Task<ServiceResponse<int>> UpdateTeacher(int id, TeacherCreateDto teacherdto);
        Task<ServiceResponse<int>> DeleteTeacher(int id);

        Task<ServiceResponse<List<TeacherResponseDto>>> GetAllTeachers();

        Task<ServiceResponse<TeacherResponseDto?>> GetTeacherById(int id);

        Task<ServiceResponse<TeacherDetailDto?>> GetTeacherDetails(int id);

    }
}

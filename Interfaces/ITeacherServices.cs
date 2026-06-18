using CRUD.DTOs;

namespace CRUD.Interfaces
{
    public interface ITeacherServices
    {

        Task<int> CreateTeacher(TeacherCreateDto teacherdto);
        Task<int> UpdateTeacher(int id, TeacherCreateDto teacherdto);
        Task<int> DeleteTeacher(int id);

        Task<List<TeacherResponseDto>> GetAllTeachers();

        Task<TeacherResponseDto?> GetTeacherById(int id);

        Task<TeacherDetailDto?> GetTeacherDetails(int id);

    }
}

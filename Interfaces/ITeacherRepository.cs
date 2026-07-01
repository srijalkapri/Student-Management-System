using CRUD.Models;
using CRUD.DTOs;

namespace CRUD.Interfaces
{
    public interface ITeacherRepository
    {
        Task<int> CreateTeacher(Teacher teacher);
        Task<int> UpdateTeacher(int id, string name, string email, string phoneNo);
        Task<int> DeleteTeacher(int id);
        Task<int> RestoreTeacher(int id);
        Task<bool> IsTeacherClassTeacher(int teacherId);
        Task<bool> TeacherExists(int teacherId);

        Task<List<TeacherResponseDto>> GetAllTeachers();
        Task<TeacherResponseDto?> GetTeacherById(int id);
        Task<TeacherDetailsDto?> GetTeacherDetails(int id);
        Task<PagedResult<TeacherResponseDto>> GetTeachersPagedAsync(PaginationParameters parameters);
    }
}

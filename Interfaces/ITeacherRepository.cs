using CRUD.Models;
using CRUD.DTOs;

namespace CRUD.Interfaces
{
    public interface ITeacherRepository
    {

        Task<int> CreateTeacher(Teacher teacher);
        Task<int> UpdateTeacher(Teacher teacher);

        Task<int> DeleteTeacher(int id);

       Task<List <TeacherResponseDto>> GetAllTeachers();
        Task<TeacherResponseDto?> GetTeacherById(int id);

        Task<TeacherDetailDto?> GetTeacherDetails(int id);

    }
}

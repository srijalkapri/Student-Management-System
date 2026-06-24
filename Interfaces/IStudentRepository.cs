using CRUD.DTOs;
using CRUD.Models;

namespace CRUD.Interfaces
{
    public interface IStudentRepository
    {
        Task<int> CreateStudent(Student student);
        Task<int> UpdateStudent(int id, string name, string email, string phoneNo, int gradeId);
        Task<int> DeleteStudent(int id);

        Task<List<StudentDetailsDto>> GetAllStudents();
        Task<StudentDetailsDto?> GetStudentById(int id);
    }
}

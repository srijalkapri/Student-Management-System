using CRUD.DTOs;
using CRUD.Models;


namespace CRUD.Interfaces
{
    public interface IStudentRepository
    {
        Task<int> CreateStudent(Student student);
        Task<bool> UpdateStudent(Student student);
        Task<bool> DeleteStudent(int id);

        Task<List<Student>> GetAllStudents();
        Task<StudentDetailsDto?> GetStudentById(int id);

    }
}

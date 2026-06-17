using CRUD.DTOs;
using CRUD.Models;


namespace CRUD.Interfaces
{
    public interface IStudentRepository
    {
        Task<int> CreateStudent(Student student);
        Task<int> UpdateStudent(Student student);
        Task<int> DeleteStudent(int id);

        Task<List<Student>> GetAllStudents();
        Task<StudentDetailsDto?> GetStudentById(int id);

    }
}


using CRUD.DTOs;

namespace CRUD.Interfaces
{
    public interface IStudentService
    {

        Task<int> CreateStudent(StudentCreate Studentdto);
        Task<int> UpdateStudent(int id, StudentCreate Studentdto);

        Task<int> DeleteStudent(int id);

        Task<List <StudentDetailsDto>> GetAllStudents();


        Task <StudentDetailsDto?> GetStudentById(int id);
    }
}

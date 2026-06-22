
using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface IStudentService
    {

        Task<ServiceResponse<int>> CreateStudent(StudentCreate Studentdto);
        Task<ServiceResponse<int>> UpdateStudent(int id, StudentCreate Studentdto);

        Task<ServiceResponse<int>> DeleteStudent(int id);

        Task<ServiceResponse<List<StudentDetailsDto>>> GetAllStudents();


        Task<ServiceResponse<StudentDetailsDto?>> GetStudentById(int id);
    }
}

using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface IStudentService
    {

        Task<ServiceResponse<int>> CreateStudent(StudentCreateDto studentDto);
        Task<ServiceResponse<int>> UpdateStudent(int id, StudentCreateDto studentDto);
        Task<ServiceResponse<int>> DeleteStudent(int id);
        Task<ServiceResponse<List<StudentDetailsDto>>> GetAllStudents();
        Task<ServiceResponse<StudentDetailsDto?>> GetStudentById(int id);
    }
}

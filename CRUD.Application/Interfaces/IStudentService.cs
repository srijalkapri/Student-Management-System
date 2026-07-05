using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IStudentService
    {

        Task<ServiceResponse<int>> CreateStudent(StudentCreateDto studentDto);
        Task<ServiceResponse<int>> UpdateStudent(int id, StudentCreateDto studentDto);
        Task<ServiceResponse<int>> DeleteStudent(int id);
        Task<ServiceResponse<int>> RestoreStudent(int id);
        Task<ServiceResponse<List<StudentDetailsDto>>> GetAllStudents();
        Task<ServiceResponse<StudentDetailsDto?>> GetStudentById(int id);
        Task<ServiceResponse<List<StudentDetailsDto>>> GetStudentsByGradeId(int gradeId);
        Task<ServiceResponse<PromoteStudentsResponseDto>> PreviewPromotion(PromoteStudentsRequestDto request);
        Task<ServiceResponse<PromoteStudentsResponseDto>> PromoteStudents(PromoteStudentsRequestDto request);
        Task<ServiceResponse<PagedResult<StudentDetailsDto>>> GetStudentsPaged(PaginationParameters parameters);
        Task<ServiceResponse<PagedResult<StudentDetailsDto>>> GetStudentsByGradeIdPaged(int gradeId, PaginationParameters parameters);
    }
}

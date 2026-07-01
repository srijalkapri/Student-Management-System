using CRUD.DTOs;
using CRUD.Models;

namespace CRUD.Interfaces
{
    public interface IStudentRepository
    {
        Task<int> CreateStudent(Student student);
        Task<int> UpdateStudent(int id, string name, string email, string phoneNo, int gradeId);
        Task<int> DeleteStudent(int id);
        Task<int> RestoreStudent(int id);

        Task<List<StudentDetailsDto>> GetAllStudents();
        Task<StudentDetailsDto?> GetStudentById(int id);
        Task<List<StudentDetailsDto>> GetStudentsByGradeId(int gradeId);
        Task<PromoteStudentsResponseDto> PreviewPromotion(int fromGradeId, int toGradeId, List<int>? studentIds);
        Task<PromoteStudentsResponseDto> PromoteStudents(int fromGradeId, int toGradeId, List<int>? studentIds);
        Task<PagedResult<StudentDetailsDto>> GetStudentsPaged(PaginationParameters parameters);
        Task<PagedResult<StudentDetailsDto>> GetStudentsByGradeIdPaged(int gradeId, PaginationParameters parameters);
    }
}

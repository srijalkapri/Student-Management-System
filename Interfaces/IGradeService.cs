using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface IGradeService
    {
        Task<ServiceResponse<int>> CreateGrade(GradeCreateDto gradeDto);
        Task<ServiceResponse<int>> UpdateGrade(int id, GradeCreateDto gradeDto);
        Task<ServiceResponse<int>> DeleteGrade(int id);
        Task<ServiceResponse<List<GradeResponseDto>>> GetAllGrades();
        Task<ServiceResponse<GradeResponseDto?>> GetGradeById(int id);
        Task<ServiceResponse<PagedResult<GradeResponseDto>>> GetGradesPagedAsync(PaginationParameters parameters);
    }
}

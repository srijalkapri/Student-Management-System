using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IGradeService
    {
        Task<ServiceResponse<int>> CreateGrade(GradeCreateDto gradeDto);
        Task<ServiceResponse<int>> UpdateGrade(int id, GradeCreateDto gradeDto);
        Task<ServiceResponse<int>> DeleteGrade(int id);
        Task<ServiceResponse<List<GradeResponseDto>>> GetAllGrades();
        Task<ServiceResponse<GradeResponseDto?>> GetGradeById(int id);
        Task<ServiceResponse<PagedResult<GradeResponseDto>>> GetGradesPaged(PaginationParameters parameters);
    }
}

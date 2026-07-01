using CRUD.DTOs;
using CRUD.Models;

namespace CRUD.Interfaces
{
    public interface IGradeRepository
    {
        Task<int> CreateGrade(Grade grade);
        Task<int> UpdateGrade(Grade grade);
        Task<int> DeleteGrade(int id);
        Task<List<GradeResponseDto>> GetAllGrades();
        Task<GradeResponseDto?> GetGradeById(int id);
        Task<PagedResult<GradeResponseDto>> GetGradesPaged(PaginationParameters parameters);
    }
}

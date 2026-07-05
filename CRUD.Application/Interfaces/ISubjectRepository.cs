using CRUD.Domain.Models;
using CRUD.Application.DTOs;

namespace CRUD.Application.Interfaces
{
    public interface ISubjectRepository
    {
        Task<int> CreateSubject(Subject subject);
        Task<int> UpdateSubject(Subject subject);
        Task<int> DeleteSubject(int id);
        Task<List<SubjectResponseDto>> GetAllSubjects();
        Task<SubjectResponseDto?> GetSubjectById(int id);
        Task<PagedResult<SubjectResponseDto>> GetSubjectsPaged(PaginationParameters parameters);
    }
}

using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface ISubjectService
    {
        Task<ServiceResponse<int>> CreateSubject(SubjectCreateDto subjectDto);
        Task<ServiceResponse<int>> UpdateSubject(int id, SubjectCreateDto subjectDto);
        Task<ServiceResponse<int>> DeleteSubject(int id);
        Task<ServiceResponse<List<SubjectResponseDto>>> GetAllSubjects();
        Task<ServiceResponse<SubjectResponseDto?>> GetSubjectById(int id);
        Task<ServiceResponse<PagedResult<SubjectResponseDto>>> GetSubjectsPaged(PaginationParameters parameters);
    }
}

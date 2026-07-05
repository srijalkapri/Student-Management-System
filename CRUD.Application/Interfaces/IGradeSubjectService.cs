using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IGradeSubjectService
    {
        Task<ServiceResponse<int>> CreateGradeSubject(GradeSubjectCreateDto gradeSubjectDto);
        Task<ServiceResponse<int>> UpdateGradeSubject(int id, GradeSubjectCreateDto gradeSubjectDto);
        Task<ServiceResponse<int>> DeleteGradeSubject(int id);
        Task<ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>> GetAllGradeSubjects(bool? isOptional = null);
        Task<ServiceResponse<GradeSubjectWithTeachersResponseDto?>> GetGradeSubjectById(int id);
        Task<ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>> GetGradeSubjectsByGradeId(int gradeId, bool? isOptional = null);
        Task<ServiceResponse<PagedResult<GradeSubjectWithTeachersResponseDto>>> GetGradeSubjectsPaged(PaginationParameters parameters);
        Task<ServiceResponse<PagedResult<GradeSubjectWithTeachersResponseDto>>> GetGradeSubjectsByGradeIdPaged(int gradeId, PaginationParameters parameters);
    }
}

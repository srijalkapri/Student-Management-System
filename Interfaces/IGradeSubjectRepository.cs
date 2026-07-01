using CRUD.DTOs;
using CRUD.Models;

namespace CRUD.Interfaces
{
    public interface IGradeSubjectRepository
    {
        Task<int> CreateGradeSubject(GradeSubject gradeSubject);
        Task<int> UpdateGradeSubject(int id, GradeSubjectCreateDto gradeSubjectDto);
        Task<int> DeleteGradeSubject(int id);
        Task<List<GradeSubjectWithTeachersResponseDto>> GetAllGradeSubjects(bool? isOptional = null);
        Task<GradeSubjectWithTeachersResponseDto?> GetGradeSubjectById(int id);
        Task<List<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsByGradeId(int gradeId, bool? isOptional = null);
        Task<PagedResult<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsPagedAsync(PaginationParameters parameters);
        Task<PagedResult<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsByGradeIdPagedAsync(int gradeId, PaginationParameters parameters);
        Task<List<GradeSubject>> GetGradeSubjectsByGradeIdEntities(int gradeId);
        Task<GradeSubject?> GetGradeSubjectEntityById(int id);
        Task<bool> GradeSubjectExistsForGrade(int gradeSubjectId, int gradeId);
    }
}

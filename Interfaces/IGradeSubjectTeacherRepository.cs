using CRUD.DTOs;

namespace CRUD.Interfaces
{
    public interface IGradeSubjectTeacherRepository
    {
        Task<List<GradeSubjectTeacherResponseDto>> GetAll();
        Task<GradeSubjectTeacherResponseDto?> GetById(int id);
        Task<int> Create(GradeSubjectTeacherCreateDto gradeSubjectTeacher);
        Task<int> Update(int id, GradeSubjectTeacherCreateDto gradeSubjectTeacher);
        Task<int> Delete(int id);
        Task<bool> GradeSubjectExists(int gradeSubjectId);
        Task<bool> TeacherExists(int teacherId);
        Task<PagedResult<GradeSubjectTeacherResponseDto>> GetPagedAsync(PaginationParameters parameters);
    }
}

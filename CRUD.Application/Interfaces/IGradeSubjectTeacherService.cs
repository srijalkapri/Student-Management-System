using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IGradeSubjectTeacherService
    {
        Task<ServiceResponse<int>> CreateGradeSubjectTeacher(GradeSubjectTeacherCreateDto dto);
        Task<ServiceResponse<int>> UpdateGradeSubjectTeacher(int id, GradeSubjectTeacherCreateDto dto);
        Task<ServiceResponse<int>> DeleteGradeSubjectTeacher(int id);
        Task<ServiceResponse<List<GradeSubjectTeacherResponseDto>>> GetAllGradeSubjectTeachers();
        Task<ServiceResponse<GradeSubjectTeacherResponseDto>> GetGradeSubjectTeacherById(int id);
        Task<ServiceResponse<PagedResult<GradeSubjectTeacherResponseDto>>> GetGradeSubjectTeachersPaged(PaginationParameters parameters);
    }
}
using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface IGradeSubjectTeacherService
    {
        Task<ServiceResponse<int>> CreateGradeSubjectTeacher(GradeSubjectTeacherCreateDto dto);
        Task<ServiceResponse<int>> UpdateGradeSubjectTeacher(int id, GradeSubjectTeacherCreateDto dto);
        Task<ServiceResponse<int>> DeleteGradeSubjectTeacher(int id);
        Task<ServiceResponse<List<GradeSubjectTeacherResponseDto>>> GetAllGradeSubjectTeachers();
        Task<ServiceResponse<GradeSubjectTeacherResponseDto>> GetGradeSubjectTeacherById(int id);
    }
}
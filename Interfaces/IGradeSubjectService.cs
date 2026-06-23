using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface IGradeSubjectService
    {
        Task<ServiceResponse<int>> CreateGradeSubject(GradeSubjectCreateDto gradeSubjectDto);
        Task<ServiceResponse<int>> UpdateGradeSubject(int id, GradeSubjectCreateDto gradeSubjectDto);
        Task<ServiceResponse<int>> DeleteGradeSubject(int id);
        Task<ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>> GetAllGradeSubjects();
        Task<ServiceResponse<GradeSubjectWithTeachersResponseDto?>> GetGradeSubjectById(int id);
    }
}

using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IStudentPortalService
    {
        Task<ServiceResponse<StudentPortalOverviewDto>> GetOverview(int userId);
        Task<ServiceResponse<StudentProfileDto>> GetMyProfile(int userId);
        Task<ServiceResponse<GradeResponseDto>> GetMyGrade(int userId);
        Task<ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>> GetMySubjects(int userId);
        Task<ServiceResponse<List<TeacherResponseDto>>> GetMyTeachers(int userId);
    }
}

using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface ITeacherPortalService
    {
        Task<ServiceResponse<TeacherPortalOverviewDto>> GetOverview(int userId);
        Task<ServiceResponse<TeacherResponseDto>> GetMyProfile(int userId);
        Task<ServiceResponse<List<GradeResponseDto>>> GetMyClasses(int userId);
        Task<ServiceResponse<List<StudentDetailsDto>>> GetMyStudents(int userId);
        Task<ServiceResponse<List<TeacherSubjectAssignmentDto>>> GetMySubjects(int userId);
    }
}

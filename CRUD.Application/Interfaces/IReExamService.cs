using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IReExamService
    {
        Task<ServiceResponse<string>> Apply(int userId, ApplyReExamRequestDto request);
        Task<ServiceResponse<List<ReExamRequestDto>>> GetMyRequests(int userId);
        Task<ServiceResponse<List<ReExamRequestDto>>> GetPendingRequestApprovals();
        Task<ServiceResponse<List<ReExamRequestDto>>> GetPendingMarksApprovals();
        Task<ServiceResponse<ReExamRequestDto>> GetById(int id);
        Task<ServiceResponse<string>> ApproveRequest(int id, int adminUserId, string? comment);
        Task<ServiceResponse<string>> RejectRequest(int id, int adminUserId, string? comment);
        Task<ServiceResponse<List<ReExamRequestDto>>> GetTeacherReExams(int userId);
        Task<ServiceResponse<string>> SubmitMarks(int userId, int reExamId, TeacherSubmitReExamMarksDto request);
        Task<ServiceResponse<string>> ApproveMarks(int id, int adminUserId, string? comment);
        Task<ServiceResponse<string>> RejectMarks(int id, int adminUserId, string? comment);
    }
}

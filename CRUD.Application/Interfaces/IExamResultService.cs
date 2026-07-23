using CRUD.Application.DTOs;
using CRUD.Application.Responses;

namespace CRUD.Application.Interfaces
{
    public interface IExamResultService
    {
        Task<ServiceResponse<List<TeacherExamSessionDto>>> GetTeacherExamSessions(int userId);
        Task<ServiceResponse<TeacherExamResultBatchDto>> GetTeacherExamResults(int userId, int examSessionId);
        Task<ServiceResponse<string>> SaveDraft(int userId, TeacherSaveExamResultsRequestDto request);
        Task<ServiceResponse<string>> SubmitForApproval(int userId, TeacherSaveExamResultsRequestDto request);
        Task<ServiceResponse<List<AdminPendingExamResultDto>>> GetPendingApprovals();
        Task<ServiceResponse<AdminExamResultReviewDto>> GetBatchReview(int batchId);
        Task<ServiceResponse<string>> ApproveBatch(int batchId, int adminUserId, string? comment);
        Task<ServiceResponse<string>> RejectBatch(int batchId, int adminUserId, string? comment);
        Task<ServiceResponse<AdminScheduleMarksDto>> GetMarksBySchedule(int examScheduleId);
        Task<ServiceResponse<AdminStudentMarksRecordDto>> GetMarksByStudent(int studentId, int? examScheduleId);
        Task<ServiceResponse<List<StudentExamResultScheduleDto>>> GetStudentResults(int userId, int? examScheduleId);
    }
}


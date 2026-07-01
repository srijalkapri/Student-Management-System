using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Interfaces
{
    public interface IExamService
    {
        Task<ServiceResponse<List<ExamScheduleResponseDto>>> GetAllSchedules();
        Task<ServiceResponse<ExamScheduleResponseDto?>> GetScheduleById(int id);
        Task<ServiceResponse<List<ExamScheduleResponseDto>>> GetSchedulesByGrade(int gradeId);
        Task<ServiceResponse<int>> CreateSchedule(ExamScheduleRequestDto request);
        Task<ServiceResponse<int>> UpdateSchedule(int id, UpdateExamScheduleRequestDto request);
        Task<ServiceResponse<int>> DeleteSchedule(int id);
        Task<ServiceResponse<int>> UpdateSession(int id, ExamSessionRequestDto request);
        Task<ServiceResponse<int>> AddSession(AddExamSessionRequestDto request);
        Task<ServiceResponse<int>> DeleteSession(int id);
        Task<ServiceResponse<bool>> BulkUpdateSessions(BulkUpdateSessionsRequestDto request);
        Task<ServiceResponse<PagedResult<ExamScheduleResponseDto>>> GetSchedulesPaged(PaginationParameters parameters);
        Task<ServiceResponse<PagedResult<ExamScheduleResponseDto>>> GetSchedulesByGradePaged(int gradeId, PaginationParameters parameters);
    }
}

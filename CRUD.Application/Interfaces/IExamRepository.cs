using CRUD.Application.DTOs;
using CRUD.Domain.Models;

namespace CRUD.Application.Interfaces
{
    public interface IExamRepository
    {
        Task<List<ExamScheduleResponseDto>> GetAllSchedules();
        Task<ExamScheduleResponseDto?> GetScheduleById(int id);
        Task<List<ExamScheduleResponseDto>> GetSchedulesByGrade(int gradeId);
        Task<int> CreateSchedule(ExamSchedule examSchedule, List<ExamSession>? sessions);
        Task<int> UpdateSchedule(ExamSchedule examSchedule);
        Task<int> DeleteSchedule(int id);
        Task<int> UpdateSession(ExamSession session);
        Task<int> AddSession(ExamSession session);
        Task<int> DeleteSession(int id);
        Task BulkUpdateSessions(List<ExamSession> sessions);
        Task<ExamSchedule?> GetScheduleEntityById(int id);
        Task<ExamSession?> GetSessionEntityById(int id);
        Task<List<ExamSession>> GetSessionsByScheduleId(int scheduleId);
        Task<PagedResult<ExamScheduleResponseDto>> GetSchedulesPaged(PaginationParameters parameters);
        Task<PagedResult<ExamScheduleResponseDto>> GetSchedulesByGradePaged(int gradeId, PaginationParameters parameters);
    }
}

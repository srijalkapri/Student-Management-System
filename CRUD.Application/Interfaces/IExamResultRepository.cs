using CRUD.Application.DTOs;
using CRUD.Domain.Models;

namespace CRUD.Application.Interfaces
{
    public interface IExamResultRepository
    {
        Task<List<TeacherExamSessionDto>> GetTeacherExamSessions(int teacherId);
        Task<TeacherExamResultBatchDto?> GetTeacherBatchBySession(int teacherId, int examSessionId);
        Task<List<StudentDetailsDto>> GetStudentsByExamSession(int examSessionId);
        Task<ExamResultBatch?> GetBatchEntityBySessionTeacher(int examSessionId, int teacherId);
        Task<ExamResultBatch?> GetBatchEntityById(int batchId);
        Task<int> CreateBatch(ExamResultBatch batch);
        Task ReplaceBatchItems(int batchId, List<ExamResultItem> items);
        Task UpdateBatch(ExamResultBatch batch);
        Task<List<AdminPendingExamResultDto>> GetPendingBatches();
        Task<AdminExamResultReviewDto?> GetBatchReviewById(int batchId);
        Task<AdminScheduleMarksDto?> GetApprovedMarksBySchedule(int examScheduleId);
        Task<List<StudentExamResultScheduleDto>> GetStudentApprovedResults(int studentId, int? examScheduleId);
    }
}


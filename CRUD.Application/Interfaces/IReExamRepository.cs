using CRUD.Application.DTOs;
using CRUD.Domain.Models;

namespace CRUD.Application.Interfaces
{
    public interface IReExamRepository
    {
        Task<ReExamRequest?> GetById(int id);
        Task<ReExamRequestDto?> GetDtoById(int id);
        Task<List<ReExamRequestDto>> GetByStudent(int studentId);
        Task<List<ReExamRequestDto>> GetPendingRequestApprovals();
        Task<List<ReExamRequestDto>> GetPendingMarksApprovals();
        Task<List<ReExamRequestDto>> GetForTeacher(int teacherId);
        Task<ExamResultItem?> GetApprovedResultItem(int studentId, int examSessionId);
        Task<bool> HasOpenRequest(int studentId, int examSessionId);
        Task<int> CountCompletedAttempts(int studentId, int examSessionId);
        Task<int> Create(ReExamRequest request);
        Task Update(ReExamRequest request);
        Task<Dictionary<(int StudentId, int ExamSessionId), ReExamRequest>> GetApprovedMarksLookup(
            IEnumerable<int> studentIds,
            IEnumerable<int> examSessionIds);
        Task<Dictionary<int, ReExamRequest>> GetLatestRequestsBySessionForStudent(int studentId);
        Task<bool> IsTeacherAssignedToSession(int teacherId, int examSessionId);
    }
}

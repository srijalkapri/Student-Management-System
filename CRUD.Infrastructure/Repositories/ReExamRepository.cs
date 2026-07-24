using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using CRUD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories
{
    public class ReExamRepository : IReExamRepository
    {
        private static readonly ReExamRequestStatus[] OpenStatuses =
        [
            ReExamRequestStatus.Requested,
            ReExamRequestStatus.Approved,
            ReExamRequestStatus.MarksSubmitted,
            ReExamRequestStatus.MarksRejected
        ];

        private readonly ApplicationDbContext _context;

        public ReExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ReExamRequest?> GetById(int id)
        {
            return _context.ReExamRequests.FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<ReExamRequestDto?> GetDtoById(int id)
        {
            return MapQuery(_context.ReExamRequests.Where(r => r.Id == id))
                .FirstOrDefaultAsync();
        }

        public Task<List<ReExamRequestDto>> GetByStudent(int studentId)
        {
            return MapQuery(_context.ReExamRequests.Where(r => r.StudentId == studentId))
                .OrderByDescending(r => r.CreatedAtUtc)
                .ToListAsync();
        }

        public Task<List<ReExamRequestDto>> GetPendingRequestApprovals()
        {
            return MapQuery(_context.ReExamRequests.Where(r => r.Status == ReExamRequestStatus.Requested))
                .OrderByDescending(r => r.CreatedAtUtc)
                .ToListAsync();
        }

        public Task<List<ReExamRequestDto>> GetPendingMarksApprovals()
        {
            return MapQuery(_context.ReExamRequests.Where(r => r.Status == ReExamRequestStatus.MarksSubmitted))
                .OrderByDescending(r => r.MarksSubmittedAtUtc)
                .ToListAsync();
        }

        public async Task<List<ReExamRequestDto>> GetForTeacher(int teacherId)
        {
            var sessionIds = await _context.ExamSessions
                .Where(es => es.GradeSubject.GradeSubjectTeachers.Any(gst => gst.TeacherId == teacherId))
                .Select(es => es.Id)
                .ToListAsync();

            return await MapQuery(_context.ReExamRequests.Where(r =>
                    sessionIds.Contains(r.ExamSessionId) &&
                    (r.Status == ReExamRequestStatus.Approved ||
                     r.Status == ReExamRequestStatus.MarksSubmitted ||
                     r.Status == ReExamRequestStatus.MarksRejected ||
                     r.Status == ReExamRequestStatus.MarksApproved)))
                .OrderByDescending(r => r.CreatedAtUtc)
                .ToListAsync();
        }

        public Task<ExamResultItem?> GetApprovedResultItem(int studentId, int examSessionId)
        {
            return _context.ExamResultItems
                .Include(i => i.ExamResultBatch)
                .FirstOrDefaultAsync(i =>
                    i.StudentId == studentId &&
                    i.ExamResultBatch.ExamSessionId == examSessionId &&
                    i.ExamResultBatch.Status == ExamResultStatus.Approved);
        }

        public Task<bool> HasOpenRequest(int studentId, int examSessionId)
        {
            return _context.ReExamRequests.AnyAsync(r =>
                r.StudentId == studentId &&
                r.ExamSessionId == examSessionId &&
                OpenStatuses.Contains(r.Status));
        }

        public Task<int> CountCompletedAttempts(int studentId, int examSessionId)
        {
            return _context.ReExamRequests.CountAsync(r =>
                r.StudentId == studentId &&
                r.ExamSessionId == examSessionId &&
                r.Status == ReExamRequestStatus.MarksApproved);
        }

        public async Task<int> Create(ReExamRequest request)
        {
            _context.ReExamRequests.Add(request);
            await _context.SaveChangesAsync();
            return request.Id;
        }

        public async Task Update(ReExamRequest request)
        {
            request.UpdatedAtUtc = DateTime.UtcNow;
            _context.ReExamRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<(int StudentId, int ExamSessionId), ReExamRequest>> GetApprovedMarksLookup(
            IEnumerable<int> studentIds,
            IEnumerable<int> examSessionIds)
        {
            var studentIdList = studentIds.Distinct().ToList();
            var sessionIdList = examSessionIds.Distinct().ToList();

            var rows = await _context.ReExamRequests
                .Where(r =>
                    r.Status == ReExamRequestStatus.MarksApproved &&
                    studentIdList.Contains(r.StudentId) &&
                    sessionIdList.Contains(r.ExamSessionId))
                .ToListAsync();

            return rows
                .GroupBy(r => (r.StudentId, r.ExamSessionId))
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.AttemptNumber).First());
        }

        public async Task<Dictionary<int, ReExamRequest>> GetLatestRequestsBySessionForStudent(int studentId)
        {
            var rows = await _context.ReExamRequests
                .Where(r => r.StudentId == studentId)
                .ToListAsync();

            return rows
                .GroupBy(r => r.ExamSessionId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.Id).First());
        }

        public Task<bool> IsTeacherAssignedToSession(int teacherId, int examSessionId)
        {
            return _context.ExamSessions.AnyAsync(es =>
                es.Id == examSessionId &&
                es.GradeSubject.GradeSubjectTeachers.Any(gst => gst.TeacherId == teacherId));
        }

        private static IQueryable<ReExamRequestDto> MapQuery(IQueryable<ReExamRequest> query)
        {
            return query.Select(r => new ReExamRequestDto
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student.Name,
                ExamSessionId = r.ExamSessionId,
                ExamScheduleId = r.ExamSession.ExamScheduleId,
                ExamTitle = r.ExamSession.ExamSchedule.Title,
                GradeName = r.ExamSession.ExamSchedule.Grade.ClassName,
                SubjectName = r.ExamSession.GradeSubject.Subject.Name,
                AttemptNumber = r.AttemptNumber,
                Status = r.Status.ToString(),
                StudentReason = r.StudentReason,
                AdminComment = r.AdminComment,
                ReviewedAtUtc = r.ReviewedAtUtc,
                OriginalMarksObtained = r.OriginalResultItem.MarksObtained,
                OriginalTotalMarks = r.OriginalResultItem.TotalMarks,
                OriginalIsAbsent = r.OriginalResultItem.IsAbsent,
                TeacherId = r.TeacherId,
                TeacherName = r.Teacher != null ? r.Teacher.Name : null,
                MarksObtained = r.MarksObtained,
                TotalMarks = r.TotalMarks,
                IsAbsent = r.IsAbsent,
                MarksRemarks = r.MarksRemarks,
                MarksSubmittedAtUtc = r.MarksSubmittedAtUtc,
                MarksReviewComment = r.MarksReviewComment,
                CreatedAtUtc = r.CreatedAtUtc
            });
        }
    }
}

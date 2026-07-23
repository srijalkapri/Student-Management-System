using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using CRUD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories
{
    public class ExamResultRepository : IExamResultRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamResultRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeacherExamSessionDto>> GetTeacherExamSessions(int teacherId)
        {
            return await _context.ExamSessions
                .Where(es =>
                    es.ExamSchedule.Status == ExamScheduleStatus.Published &&
                    es.GradeSubject.GradeSubjectTeachers.Any(gst => gst.TeacherId == teacherId))
                .Select(es => new TeacherExamSessionDto
                {
                    ExamSessionId = es.Id,
                    ExamScheduleId = es.ExamScheduleId,
                    ExamTitle = es.ExamSchedule.Title,
                    AcademicYear = es.ExamSchedule.AcademicYear,
                    GradeId = es.ExamSchedule.GradeId,
                    GradeName = es.ExamSchedule.Grade.ClassName,
                    GradeSubjectId = es.GradeSubjectId,
                    SubjectId = es.GradeSubject.SubjectId,
                    SubjectName = es.GradeSubject.Subject.Name,
                    ExamDate = es.ExamDate,
                    StartTime = es.StartTime,
                    EndTime = es.EndTime,
                    BatchId = _context.ExamResultBatches
                        .Where(b => b.ExamSessionId == es.Id && b.TeacherId == teacherId)
                        .Select(b => (int?)b.Id)
                        .FirstOrDefault(),
                    ResultStatus = _context.ExamResultBatches
                        .Where(b => b.ExamSessionId == es.Id && b.TeacherId == teacherId)
                        .Select(b => b.Status.ToString())
                        .FirstOrDefault()
                })
                .OrderBy(x => x.ExamDate)
                .ThenBy(x => x.SubjectName)
                .ToListAsync();
        }

        public async Task<TeacherExamResultBatchDto?> GetTeacherBatchBySession(int teacherId, int examSessionId)
        {
            return await _context.ExamResultBatches
                .Where(b => b.TeacherId == teacherId && b.ExamSessionId == examSessionId)
                .Select(b => new TeacherExamResultBatchDto
                {
                    BatchId = b.Id,
                    ExamSessionId = b.ExamSessionId,
                    Status = b.Status.ToString(),
                    SubmittedAtUtc = b.SubmittedAtUtc,
                    ReviewedAtUtc = b.ReviewedAtUtc,
                    ReviewComment = b.ReviewComment,
                    Items = b.Items
                        .OrderBy(i => i.Student.Name)
                        .Select(i => new TeacherExamResultItemDto
                        {
                            StudentId = i.StudentId,
                            StudentName = i.Student.Name,
                            MarksObtained = i.MarksObtained,
                            TotalMarks = i.TotalMarks,
                            IsAbsent = i.IsAbsent,
                            Remarks = i.Remarks
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<StudentDetailsDto>> GetStudentsByExamSession(int examSessionId)
        {
            var gradeId = await _context.ExamSessions
                .Where(s => s.Id == examSessionId)
                .Select(s => s.ExamSchedule.GradeId)
                .FirstOrDefaultAsync();

            if (gradeId == 0)
            {
                return new List<StudentDetailsDto>();
            }

            return await _context.Students
                .Where(s => s.GradeId == gradeId)
                .OrderBy(s => s.Name)
                .Select(s => new StudentDetailsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNo = s.PhoneNo,
                    GradeId = s.GradeId,
                    GradeName = s.Grade.ClassName
                })
                .ToListAsync();
        }

        public async Task<ExamResultBatch?> GetBatchEntityBySessionTeacher(int examSessionId, int teacherId)
        {
            return await _context.ExamResultBatches
                .FirstOrDefaultAsync(b => b.ExamSessionId == examSessionId && b.TeacherId == teacherId);
        }

        public async Task<ExamResultBatch?> GetBatchEntityById(int batchId)
        {
            return await _context.ExamResultBatches
                .FirstOrDefaultAsync(b => b.Id == batchId);
        }

        public async Task<int> CreateBatch(ExamResultBatch batch)
        {
            _context.ExamResultBatches.Add(batch);
            await _context.SaveChangesAsync();
            return batch.Id;
        }

        public async Task ReplaceBatchItems(int batchId, List<ExamResultItem> items)
        {
            var existingItems = await _context.ExamResultItems
                .Where(i => i.ExamResultBatchId == batchId)
                .ToListAsync();

            if (existingItems.Count > 0)
            {
                _context.ExamResultItems.RemoveRange(existingItems);
            }

            if (items.Count > 0)
            {
                _context.ExamResultItems.AddRange(items);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatch(ExamResultBatch batch)
        {
            batch.UpdatedAtUtc = DateTime.UtcNow;
            _context.ExamResultBatches.Update(batch);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AdminPendingExamResultDto>> GetPendingBatches()
        {
            return await _context.ExamResultBatches
                .Where(b => b.Status == ExamResultStatus.PendingApproval)
                .OrderByDescending(b => b.SubmittedAtUtc)
                .Select(b => new AdminPendingExamResultDto
                {
                    BatchId = b.Id,
                    ExamSessionId = b.ExamSessionId,
                    ExamScheduleId = b.ExamSession.ExamScheduleId,
                    ExamTitle = b.ExamSession.ExamSchedule.Title,
                    GradeName = b.ExamSession.ExamSchedule.Grade.ClassName,
                    SubjectName = b.ExamSession.GradeSubject.Subject.Name,
                    TeacherId = b.TeacherId,
                    TeacherName = b.Teacher.Name,
                    StudentCount = b.Items.Count,
                    SubmittedAtUtc = b.SubmittedAtUtc
                })
                .ToListAsync();
        }

        public async Task<AdminExamResultReviewDto?> GetBatchReviewById(int batchId)
        {
            return await _context.ExamResultBatches
                .Where(b => b.Id == batchId)
                .Select(b => new AdminExamResultReviewDto
                {
                    BatchId = b.Id,
                    Status = b.Status.ToString(),
                    ExamTitle = b.ExamSession.ExamSchedule.Title,
                    GradeName = b.ExamSession.ExamSchedule.Grade.ClassName,
                    SubjectName = b.ExamSession.GradeSubject.Subject.Name,
                    TeacherName = b.Teacher.Name,
                    SubmittedAtUtc = b.SubmittedAtUtc,
                    ReviewComment = b.ReviewComment,
                    Items = b.Items
                        .OrderBy(i => i.Student.Name)
                        .Select(i => new TeacherExamResultItemDto
                        {
                            StudentId = i.StudentId,
                            StudentName = i.Student.Name,
                            MarksObtained = i.MarksObtained,
                            TotalMarks = i.TotalMarks,
                            IsAbsent = i.IsAbsent,
                            Remarks = i.Remarks
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AdminScheduleMarksDto?> GetApprovedMarksBySchedule(int examScheduleId)
        {
            var schedule = await _context.ExamSchedules
                .Where(s => s.Id == examScheduleId)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.AcademicYear,
                    GradeName = s.Grade.ClassName
                })
                .FirstOrDefaultAsync();

            if (schedule == null)
            {
                return null;
            }

            var rows = await _context.ExamResultItems
                .Where(i =>
                    i.ExamResultBatch.ExamSession.ExamScheduleId == examScheduleId &&
                    i.ExamResultBatch.Status == ExamResultStatus.Approved)
                .Select(i => new
                {
                    i.StudentId,
                    StudentName = i.Student.Name,
                    SubjectName = i.ExamResultBatch.ExamSession.GradeSubject.Subject.Name,
                    i.MarksObtained,
                    i.TotalMarks,
                    i.IsAbsent,
                    i.Remarks
                })
                .ToListAsync();

            var students = rows
                .GroupBy(x => new { x.StudentId, x.StudentName })
                .Select(g =>
                {
                    var subjects = g
                        .OrderBy(x => x.SubjectName)
                        .Select(x => new StudentExamResultSubjectDto
                        {
                            SubjectName = x.SubjectName,
                            MarksObtained = x.MarksObtained,
                            TotalMarks = x.TotalMarks,
                            IsAbsent = x.IsAbsent,
                            Remarks = x.Remarks
                        })
                        .ToList();

                    var totalMarks = subjects.Sum(s => s.TotalMarks);
                    var totalObtained = subjects.Sum(s => s.MarksObtained ?? 0m);
                    var percentage = totalMarks <= 0 ? 0 : decimal.Round((totalObtained / totalMarks) * 100m, 2);

                    return new AdminStudentMarksRowDto
                    {
                        StudentId = g.Key.StudentId,
                        StudentName = g.Key.StudentName,
                        Subjects = subjects,
                        TotalMarks = totalMarks,
                        TotalObtained = totalObtained,
                        Percentage = percentage
                    };
                })
                .OrderBy(x => x.StudentName)
                .ToList();

            return new AdminScheduleMarksDto
            {
                ExamScheduleId = schedule.Id,
                ExamTitle = schedule.Title,
                AcademicYear = schedule.AcademicYear,
                GradeName = schedule.GradeName,
                Students = students
            };
        }

        public async Task<List<StudentExamResultScheduleDto>> GetStudentApprovedResults(int studentId, int? examScheduleId)
        {
            var query = _context.ExamResultItems
                .Where(i =>
                    i.StudentId == studentId &&
                    i.ExamResultBatch.Status == ExamResultStatus.Approved);

            if (examScheduleId.HasValue)
            {
                query = query.Where(i => i.ExamResultBatch.ExamSession.ExamScheduleId == examScheduleId.Value);
            }

            var rows = await query
                .Select(i => new
                {
                    i.ExamResultBatch.ExamSession.ExamScheduleId,
                    i.ExamResultBatch.ExamSession.ExamSchedule.Title,
                    i.ExamResultBatch.ExamSession.ExamSchedule.AcademicYear,
                    SubjectName = i.ExamResultBatch.ExamSession.GradeSubject.Subject.Name,
                    i.MarksObtained,
                    i.TotalMarks,
                    i.IsAbsent,
                    i.Remarks
                })
                .ToListAsync();

            return rows
                .GroupBy(x => new { x.ExamScheduleId, x.Title, x.AcademicYear })
                .Select(g =>
                {
                    var subjects = g.Select(x => new StudentExamResultSubjectDto
                    {
                        SubjectName = x.SubjectName,
                        MarksObtained = x.MarksObtained,
                        TotalMarks = x.TotalMarks,
                        IsAbsent = x.IsAbsent,
                        Remarks = x.Remarks
                    }).ToList();

                    var totalMarks = subjects.Sum(s => s.TotalMarks);
                    var totalObtained = subjects.Sum(s => s.MarksObtained ?? 0m);
                    var percentage = totalMarks <= 0 ? 0 : decimal.Round((totalObtained / totalMarks) * 100m, 2);

                    return new StudentExamResultScheduleDto
                    {
                        ExamScheduleId = g.Key.ExamScheduleId,
                        ExamTitle = g.Key.Title,
                        AcademicYear = g.Key.AcademicYear,
                        Subjects = subjects,
                        TotalMarks = totalMarks,
                        TotalObtained = totalObtained,
                        Percentage = percentage
                    };
                })
                .OrderByDescending(x => x.ExamScheduleId)
                .ToList();
        }
    }
}

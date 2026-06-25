using CRUD.Data;
using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamScheduleResponseDto>> GetAllSchedules()
        {
            return await _context.ExamSchedules
                .Select(es => new ExamScheduleResponseDto
                {
                    Id = es.Id,
                    GradeId = es.GradeId,
                    GradeName = es.Grade.ClassName,
                    Title = es.Title,
                    AcademicYear = es.AcademicYear,
                    Status = es.Status,
                    CreatedAt = es.CreatedAt,
                    Sessions = es.ExamSessions
                        .Select(ess => new ExamSessionResponseDto
                        {
                            Id = ess.Id,
                            GradeSubjectId = ess.GradeSubjectId,
                            SubjectId = ess.GradeSubject.SubjectId,
                            SubjectName = ess.GradeSubject.Subject.Name,
                            IsOptional = ess.GradeSubject.IsOptional,
                            ExamDate = ess.ExamDate,
                            StartTime = ess.StartTime,
                            EndTime = ess.EndTime,
                            InvigilatorTeacherId = ess.InvigilatorTeacherId,
                            InvigilatorTeacherName = ess.InvigilatorTeacher != null ? ess.InvigilatorTeacher.Name : null,
                            Notes = ess.Notes
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<ExamScheduleResponseDto?> GetScheduleById(int id)
        {
            return await _context.ExamSchedules
                .Where(es => es.Id == id)
                .Select(es => new ExamScheduleResponseDto
                {
                    Id = es.Id,
                    GradeId = es.GradeId,
                    GradeName = es.Grade.ClassName,
                    Title = es.Title,
                    AcademicYear = es.AcademicYear,
                    Status = es.Status,
                    CreatedAt = es.CreatedAt,
                    Sessions = es.ExamSessions
                        .Select(ess => new ExamSessionResponseDto
                        {
                            Id = ess.Id,
                            GradeSubjectId = ess.GradeSubjectId,
                            SubjectId = ess.GradeSubject.SubjectId,
                            SubjectName = ess.GradeSubject.Subject.Name,
                            IsOptional = ess.GradeSubject.IsOptional,
                            ExamDate = ess.ExamDate,
                            StartTime = ess.StartTime,
                            EndTime = ess.EndTime,
                            InvigilatorTeacherId = ess.InvigilatorTeacherId,
                            InvigilatorTeacherName = ess.InvigilatorTeacher != null ? ess.InvigilatorTeacher.Name : null,
                            Notes = ess.Notes
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ExamScheduleResponseDto>> GetSchedulesByGrade(int gradeId)
        {
            return await _context.ExamSchedules
                .Where(es => es.GradeId == gradeId)
                .Select(es => new ExamScheduleResponseDto
                {
                    Id = es.Id,
                    GradeId = es.GradeId,
                    GradeName = es.Grade.ClassName,
                    Title = es.Title,
                    AcademicYear = es.AcademicYear,
                    Status = es.Status,
                    CreatedAt = es.CreatedAt,
                    Sessions = es.ExamSessions
                        .Select(ess => new ExamSessionResponseDto
                        {
                            Id = ess.Id,
                            GradeSubjectId = ess.GradeSubjectId,
                            SubjectId = ess.GradeSubject.SubjectId,
                            SubjectName = ess.GradeSubject.Subject.Name,
                            IsOptional = ess.GradeSubject.IsOptional,
                            ExamDate = ess.ExamDate,
                            StartTime = ess.StartTime,
                            EndTime = ess.EndTime,
                            InvigilatorTeacherId = ess.InvigilatorTeacherId,
                            InvigilatorTeacherName = ess.InvigilatorTeacher != null ? ess.InvigilatorTeacher.Name : null,
                            Notes = ess.Notes
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<int> CreateSchedule(ExamSchedule examSchedule, List<ExamSession>? sessions)
        {
            _context.ExamSchedules.Add(examSchedule);
            await _context.SaveChangesAsync();

            if (sessions != null && sessions.Any())
            {
                foreach (var session in sessions)
                {
                    session.ExamScheduleId = examSchedule.Id;
                }
                _context.ExamSessions.AddRange(sessions);
                await _context.SaveChangesAsync();
            }

            return examSchedule.Id;
        }

        public async Task<int> UpdateSchedule(ExamSchedule examSchedule)
        {
            var existing = await _context.ExamSchedules.FindAsync(examSchedule.Id);
            if (existing == null) return 0;

            existing.Title = examSchedule.Title;
            existing.AcademicYear = examSchedule.AcademicYear;
            existing.Status = examSchedule.Status;

            await _context.SaveChangesAsync();
            return existing.Id;
        }

        public async Task<int> DeleteSchedule(int id)
        {
            var schedule = await _context.ExamSchedules.FindAsync(id);
            if (schedule == null) return 0;

            _context.ExamSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<int> UpdateSession(ExamSession session)
        {
            var existing = await _context.ExamSessions.FindAsync(session.Id);
            if (existing == null) return 0;

            existing.ExamDate = session.ExamDate;
            existing.StartTime = session.StartTime;
            existing.EndTime = session.EndTime;
            existing.InvigilatorTeacherId = session.InvigilatorTeacherId;
            existing.Notes = session.Notes;

            await _context.SaveChangesAsync();
            return existing.Id;
        }

        public async Task<int> AddSession(ExamSession session)
        {
            _context.ExamSessions.Add(session);
            await _context.SaveChangesAsync();
            return session.Id;
        }

        public async Task<int> DeleteSession(int id)
        {
            var session = await _context.ExamSessions.FindAsync(id);
            if (session == null) return 0;

            _context.ExamSessions.Remove(session);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task BulkUpdateSessions(List<ExamSession> sessions)
        {
            _context.ExamSessions.UpdateRange(sessions);
            await _context.SaveChangesAsync();
        }

        public async Task<ExamSchedule?> GetScheduleEntityById(int id)
        {
            return await _context.ExamSchedules.FindAsync(id);
        }

        public async Task<ExamSession?> GetSessionEntityById(int id)
        {
            return await _context.ExamSessions.FindAsync(id);
        }
    }
}

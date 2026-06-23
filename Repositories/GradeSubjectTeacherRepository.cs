using CRUD.Data;
using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repositories
{
    public class GradeSubjectTeacherRepository : IGradeSubjectTeacherRepository
    {
        private readonly ApplicationDbContext _context;

        public GradeSubjectTeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GradeSubjectTeacherResponseDto>> GetAll()
        {
            return await _context.GradeSubjectTeachers
                .Select(gst => new GradeSubjectTeacherResponseDto
                {
                    Id = gst.Id,
                    GradeSubjectId = gst.GradeSubjectId,
                    GradeId = gst.GradeSubject.GradeId,
                    GradeName = gst.GradeSubject.Grade.ClassName,
                    SubjectId = gst.GradeSubject.SubjectId,
                    SubjectName = gst.GradeSubject.Subject.Name,
                    TeacherId = gst.TeacherId,
                    TeacherName = gst.Teacher.Name
                })
                .ToListAsync();
        }

        public async Task<GradeSubjectTeacherResponseDto?> GetById(int id)
        {
            return await _context.GradeSubjectTeachers
                .Where(gst => gst.Id == id)
                .Select(gst => new GradeSubjectTeacherResponseDto
                {
                    Id = gst.Id,
                    GradeSubjectId = gst.GradeSubjectId,
                    GradeId = gst.GradeSubject.GradeId,
                    GradeName = gst.GradeSubject.Grade.ClassName,
                    SubjectId = gst.GradeSubject.SubjectId,
                    SubjectName = gst.GradeSubject.Subject.Name,
                    TeacherId = gst.TeacherId,
                    TeacherName = gst.Teacher.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> Create(GradeSubjectTeacherCreateDto gradeSubjectTeacher)
        {
            var entity = new GradeSubjectTeacher
            {
                GradeSubjectId = gradeSubjectTeacher.GradeSubjectId,
                TeacherId = gradeSubjectTeacher.TeacherId
            };
            _context.GradeSubjectTeachers.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> Update(int id, GradeSubjectTeacherCreateDto gradeSubjectTeacher)
        {
            var existing = await _context.GradeSubjectTeachers.FindAsync(id);
            if (existing == null) return 0;

            existing.GradeSubjectId = gradeSubjectTeacher.GradeSubjectId;
            existing.TeacherId = gradeSubjectTeacher.TeacherId;
            await _context.SaveChangesAsync();
            return existing.Id;
        }

        public async Task<int> Delete(int id)
        {
            var gradeSubjectTeacher = await _context.GradeSubjectTeachers.FindAsync(id);
            if (gradeSubjectTeacher == null) return 0;

            _context.GradeSubjectTeachers.Remove(gradeSubjectTeacher);
            await _context.SaveChangesAsync();
            return gradeSubjectTeacher.Id;
        }

        public async Task<bool> GradeSubjectExists(int gradeSubjectId)
        {
            return await _context.GradeSubjects.AnyAsync(gs => gs.Id == gradeSubjectId);
        }

        public async Task<bool> TeacherExists(int teacherId)
        {
            return await _context.Teachers.AnyAsync(t => t.Id == teacherId);
        }
    }
}
using CRUD.Data;
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

        public async Task<List<GradeSubjectTeacher>> GetAll()
        {
            return await _context.GradeSubjectTeachers
                .Include(gst => gst.GradeSubject)
                    .ThenInclude(gs => gs.Grade)
                .Include(gst => gst.GradeSubject)
                    .ThenInclude(gs => gs.Subject)
                .Include(gst => gst.Teacher)
                .ToListAsync();
        }

        public async Task<GradeSubjectTeacher?> GetById(int id)
        {
            return await _context.GradeSubjectTeachers
                .Include(gst => gst.GradeSubject)
                    .ThenInclude(gs => gs.Grade)
                .Include(gst => gst.GradeSubject)
                    .ThenInclude(gs => gs.Subject)
                .Include(gst => gst.Teacher)
                .FirstOrDefaultAsync(gst => gst.Id == id);
        }

        public async Task<GradeSubjectTeacher> Create(GradeSubjectTeacher gradeSubjectTeacher)
        {
            _context.GradeSubjectTeachers.Add(gradeSubjectTeacher);
            await _context.SaveChangesAsync();
            return gradeSubjectTeacher;
        }

        public async Task<GradeSubjectTeacher?> Update(GradeSubjectTeacher gradeSubjectTeacher)
        {
            var existing = await _context.GradeSubjectTeachers.FindAsync(gradeSubjectTeacher.Id);
            if (existing == null) return null;
            existing.GradeSubjectId = gradeSubjectTeacher.GradeSubjectId;
            existing.TeacherId = gradeSubjectTeacher.TeacherId;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> Delete(int id)
        {
            var gradeSubjectTeacher = await _context.GradeSubjectTeachers.FindAsync(id);
            if (gradeSubjectTeacher == null) return false;
            _context.GradeSubjectTeachers.Remove(gradeSubjectTeacher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GradeSubjectExists(int gradeSubjectId)
        {
            return await _context.GradeSubjects.AnyAsync(gs => gs.Id == gradeSubjectId);
        }

        public async Task<bool> TeacherExistsAsync(int teacherId)
        {
            return await _context.Teachers.AnyAsync(t => t.Id == teacherId);
        }
    }

    public interface IGradeSubjectTeacherRepository
    {
        Task<List<GradeSubjectTeacher>> GetAllAsync();
        Task<GradeSubjectTeacher?> GetByIdAsync(int id);
        Task<GradeSubjectTeacher> CreateAsync(GradeSubjectTeacher gradeSubjectTeacher);
        Task<GradeSubjectTeacher?> UpdateAsync(GradeSubjectTeacher gradeSubjectTeacher);
        Task<bool> DeleteAsync(int id);
        Task<bool> GradeSubjectExistsAsync(int gradeSubjectId);
        Task<bool> TeacherExistsAsync(int teacherId);
    }
}
using CRUD.Data;
using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly ApplicationDbContext _context;

        public GradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade.Id;
        }

        public async Task<int> UpdateGrade(Grade grade)
        {
            var exists = await _context.Grades.AnyAsync(g => g.Id == grade.Id);
            if (!exists) return 0;

            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();
            return grade.Id;
        }

        public async Task<int> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return 0;

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return grade.Id;
        }

        public async Task<List<GradeResponseDto>> GetAllGrades()
        {
            return await _context.Grades
                .Include(g => g.Teacher)
                .Select(g => new GradeResponseDto
                {
                    Id = g.Id,
                    ClassName = g.ClassName,
                    Section = g.Section,
                    Subject = g.Subject,
                    TeacherId = g.TeacherId,
                    TeacherName = g.Teacher.Name
                })
                .ToListAsync();
        }

        public async Task<GradeResponseDto?> GetGradeById(int id)
        {
            return await _context.Grades
                .Include(g => g.Teacher)
                .Where(g => g.Id == id)
                .Select(g => new GradeResponseDto
                {
                    Id = g.Id,
                    ClassName = g.ClassName,
                    Section = g.Section,
                    Subject = g.Subject,
                    TeacherId = g.TeacherId,
                    TeacherName = g.Teacher.Name
                })
                .FirstOrDefaultAsync();
        }
    }
}

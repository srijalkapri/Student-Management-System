using CRUD.Data;
using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;

        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateTeacher(Teacher teacher)
        {
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task<int> UpdateTeacher(Teacher teacher)
        {
            var exists = await _context.Teachers.AnyAsync(t => t.Id == teacher.Id);
            if (!exists) return 0;

            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task<int> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return 0;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task<List<TeacherResponseDto>> GetAllTeachers()
        {
            return await _context.Teachers
                .Select(t => new TeacherResponseDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        public async Task<TeacherResponseDto?> GetTeacherById(int id)
        {
            return await _context.Teachers
                .Where(t => t.Id == id)
                .Select(t => new TeacherResponseDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TeacherDetailsDto?> GetTeacherDetails(int id)
        {
            return await _context.Teachers
                .Include(t => t.Grades)
                .Where(t => t.Id == id)
                .Select(t => new TeacherDetailsDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    AssignedGrades = t.Grades.Select(g => new GradeResponseDto
                    {
                        Id = g.Id,
                        ClassName = g.ClassName,
                        Section = g.Section,
                        Subject = g.Subject,
                        TeacherId = g.TeacherId,
                        TeacherName = t.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}

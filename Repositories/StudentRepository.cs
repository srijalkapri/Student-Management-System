using CRUD.Data;
using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateStudent(Student student)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<int> UpdateStudent(Student student)
        {
            var exists = await _context.Students.AnyAsync(s => s.Id == student.Id);
            if (!exists) return 0;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<int> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return 0;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<List<StudentDetailsDto>> GetAllStudents()
        {
            return await _context.Students
                .Include(s => s.Grade)
                .ThenInclude(g => g.Teacher)
                .Select(s => new StudentDetailsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    GradeId = s.GradeId,
                    ClassName = s.Grade.ClassName,
                    Section = s.Grade.Section,
                    Subject = s.Grade.Subject,
                    TeacherName = s.Grade.Teacher.Name
                })
                .ToListAsync();
        }

        public async Task<StudentDetailsDto?> GetStudentById(int id)
        {
            return await _context.Students
                .Include(s => s.Grade)
                .ThenInclude(g => g.Teacher)
                .Where(s => s.Id == id)
                .Select(s => new StudentDetailsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    GradeId = s.GradeId,
                    ClassName = s.Grade.ClassName,
                    Section = s.Grade.Section,
                    Subject = s.Grade.Subject,
                    TeacherName = s.Grade.Teacher.Name
                })
                .FirstOrDefaultAsync();
        }
    }
}

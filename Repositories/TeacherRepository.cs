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

        public async Task<int> UpdateTeacher(int id, string name, string email, string phoneNo)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return 0;

            teacher.Name = name;
            teacher.Email = email;
            teacher.PhoneNo = phoneNo;

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
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNo = t.PhoneNo
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
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNo = t.PhoneNo
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TeacherDetailsDto?> GetTeacherDetails(int id)
        {
            return await _context.Teachers
                .Include(t => t.GradeSubjectTeachers)
                    .ThenInclude(gst => gst.GradeSubject)
                        .ThenInclude(gs => gs.Grade)
                .Include(t => t.GradeSubjectTeachers)
                    .ThenInclude(gst => gst.GradeSubject)
                        .ThenInclude(gs => gs.Subject)
                .Where(t => t.Id == id)
                .Select(t => new TeacherDetailsDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    PhoneNo = t.PhoneNo,
                    AssignedGradeSubjectTeachers = t.GradeSubjectTeachers.Select(gst => new GradeSubjectTeacherResponseDto
                    {
                        Id = gst.Id,
                        GradeSubjectId = gst.GradeSubjectId,
                        GradeId = gst.GradeSubject.GradeId,
                        GradeName = gst.GradeSubject.Grade.ClassName,
                        SubjectId = gst.GradeSubject.SubjectId,
                        SubjectName = gst.GradeSubject.Subject.Name,
                        TeacherId = gst.TeacherId,
                        TeacherName = t.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}

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
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
            return teacher.Id;
        }


        public async Task<int> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return 0;
            }
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
                  Subject = t.Subject,
                  Grades = t.Grades,
                  TotalStudents = t.Students.Count
              }).ToListAsync();
        }

        public async Task<TeacherResponseDto?> GetTeacherById(int id)
        {
            return await _context.Teachers
                .Where(t => t.Id == id)
                .Select(t => new TeacherResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Subject = t.Subject,
                    Grades = t.Grades,
                    TotalStudents = t.Students.Count
                })
                .FirstOrDefaultAsync();
        }


        public async Task<TeacherDetailDto?> GetTeacherDetails(int id)
        {
            return await _context.Teachers
                .Where(t => t.Id == id)
                .Select(t => new TeacherDetailDto
                {
                    TeacherName = t.Name,
                    Grades = t.Grades,
                    StudentNames = t.Students.Select(s => s.Name).ToList()
                })
                .FirstOrDefaultAsync();
        }


    }


}

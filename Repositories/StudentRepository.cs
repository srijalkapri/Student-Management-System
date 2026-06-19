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

        public async Task<int>UpdateStudent(Student student)
        {

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<int> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return 0;
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<List<StudentDetailsDto>> GetAllStudents()
        {
            return await _context.Students
            .Select(s => new StudentDetailsDto

            {
                StudentId = s.Id,
                StudentName = s.Name,
                StudentGrade = s.Grade,
                TeacherName = s.Teacher.Name ?? "No Teacher Assigned",
                TeacherSubject = s.Teacher.Subject ?? "No Subject Assigned"

            }).ToListAsync()
        }

        public async Task<StudentDetailsDto?> GetStudentById(int id)
        {
            return await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentDetailsDto
                {
                    StudentId = s.Id,
                    StudentName = s.Name,
                    StudentGrade = s.Grade,
                    TeacherName = s.Teacher.Name ?? "No Teacher Assigned",
                    TeacherSubject = s.Teacher.Subject ?? "No Subject Assigned"
                })
                .FirstOrDefaultAsync();
        }


    }
}

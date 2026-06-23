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

        public async Task<int> UpdateStudent(int id, string name, int gradeId)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return 0;

            student.Name = name;
            student.GradeId = gradeId;

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
            var students = await _context.Students
                .Include(s => s.Grade)
                .ToListAsync();

            var studentDtos = new List<StudentDetailsDto>();
            foreach (var student in students)
            {
                var gradeSubjects = await _context.GradeSubjects
                    .Include(gs => gs.Subject)
                    .Where(gs => gs.GradeId == student.GradeId)
                    .ToListAsync();

                var subjects = new List<GradeSubjectWithTeachersResponseDto>();
                foreach (var gs in gradeSubjects)
                {
                    var teachers = await _context.GradeSubjectTeachers
                        .Include(gst => gst.Teacher)
                        .Where(gst => gst.GradeSubjectId == gs.Id)
                        .Select(gst => new TeacherResponseDto
                        {
                            Id = gst.TeacherId,
                            Name = gst.Teacher.Name
                        })
                        .ToListAsync();

                    subjects.Add(new GradeSubjectWithTeachersResponseDto
                    {
                        Id = gs.Id,
                        GradeId = gs.GradeId,
                        GradeName = student.Grade.ClassName,
                        SubjectId = gs.SubjectId,
                        SubjectName = gs.Subject.Name,
                        Teachers = teachers
                    });
                }

                studentDtos.Add(new StudentDetailsDto
                {
                    Id = student.Id,
                    Name = student.Name,
                    GradeId = student.GradeId,
                    GradeName = student.Grade.ClassName,
                    Subjects = subjects
                });
            }

            return studentDtos;
        }

        public async Task<StudentDetailsDto?> GetStudentById(int id)
        {
            var student = await _context.Students
                .Include(s => s.Grade)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return null;

            var gradeSubjects = await _context.GradeSubjects
                .Include(gs => gs.Subject)
                .Where(gs => gs.GradeId == student.GradeId)
                .ToListAsync();

            var subjects = new List<GradeSubjectWithTeachersResponseDto>();
            foreach (var gs in gradeSubjects)
            {
                var teachers = await _context.GradeSubjectTeachers
                    .Include(gst => gst.Teacher)
                    .Where(gst => gst.GradeSubjectId == gs.Id)
                    .Select(gst => new TeacherResponseDto
                    {
                        Id = gst.TeacherId,
                        Name = gst.Teacher.Name
                    })
                    .ToListAsync();

                subjects.Add(new GradeSubjectWithTeachersResponseDto
                {
                    Id = gs.Id,
                    GradeId = gs.GradeId,
                    GradeName = student.Grade.ClassName,
                    SubjectId = gs.SubjectId,
                    SubjectName = gs.Subject.Name,
                    Teachers = teachers
                });
            }

            return new StudentDetailsDto
            {
                Id = student.Id,
                Name = student.Name,
                GradeId = student.GradeId,
                GradeName = student.Grade.ClassName,
                Subjects = subjects
            };
        }
    }
}

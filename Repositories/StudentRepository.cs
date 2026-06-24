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

        public async Task<int> UpdateStudent(int id, string name, string email, string phoneNo, int gradeId)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return 0;

            student.Name = name;
            student.Email = email;
            student.PhoneNo = phoneNo;
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
            return await _context.Students
                .Select(student => new StudentDetailsDto
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    PhoneNo = student.PhoneNo,
                    GradeId = student.GradeId,
                    GradeName = student.Grade.ClassName,
                    ClassTeacherId = student.Grade.ClassTeacherId,
                    ClassTeacher = student.Grade.ClassTeacher != null
                        ? new TeacherResponseDto
                        {
                            Id = student.Grade.ClassTeacher.Id,
                            Name = student.Grade.ClassTeacher.Name,
                            Email = student.Grade.ClassTeacher.Email,
                            PhoneNo = student.Grade.ClassTeacher.PhoneNo
                        }
                        : null,
                    Subjects = student.Grade.GradeSubjects
                        .Select(gs => new GradeSubjectWithTeachersResponseDto
                        {
                            Id = gs.Id,
                            GradeId = gs.GradeId,
                            GradeName = student.Grade.ClassName,
                            SubjectId = gs.SubjectId,
                            SubjectName = gs.Subject.Name,
                            IsOptional = gs.IsOptional,
                            Teachers = gs.GradeSubjectTeachers
                                .Select(gst => new TeacherResponseDto
                                {
                                    Id = gst.TeacherId,
                                    Name = gst.Teacher.Name,
                                    Email = gst.Teacher.Email,
                                    PhoneNo = gst.Teacher.PhoneNo
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();
        }


        public async Task<StudentDetailsDto?> GetStudentById(int id)
        {
            return await _context.Students
                .Where(s => s.Id == id)
                .Select(student => new StudentDetailsDto
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    PhoneNo = student.PhoneNo,
                    GradeId = student.GradeId,
                    GradeName = student.Grade.ClassName,
                    ClassTeacherId = student.Grade.ClassTeacherId,
                    ClassTeacher = student.Grade.ClassTeacher != null
                        ? new TeacherResponseDto
                        {
                            Id = student.Grade.ClassTeacher.Id,
                            Name = student.Grade.ClassTeacher.Name,
                            Email = student.Grade.ClassTeacher.Email,
                            PhoneNo = student.Grade.ClassTeacher.PhoneNo
                        }
                        : null,
                    Subjects = student.Grade.GradeSubjects
                        .Select(gs => new GradeSubjectWithTeachersResponseDto
                        {
                            Id = gs.Id,
                            GradeId = gs.GradeId,
                            GradeName = student.Grade.ClassName,
                            SubjectId = gs.SubjectId,
                            SubjectName = gs.Subject.Name,
                            IsOptional = gs.IsOptional,
                            Teachers = gs.GradeSubjectTeachers
                                .Select(gst => new TeacherResponseDto
                                {
                                    Id = gst.TeacherId,
                                    Name = gst.Teacher.Name,
                                    Email = gst.Teacher.Email,
                                    PhoneNo = gst.Teacher.PhoneNo
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}

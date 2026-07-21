using CRUD.Infrastructure.Persistence;
using CRUD.Application.DTOs;
using CRUD.Application.Extensions;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories
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
            var teacher = await _context.Teachers.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return 0;
            if (teacher.IsDeleted) return -1;

            teacher.IsDeleted = true;
            teacher.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task<int> RestoreTeacher(int id)
        {
            var teacher = await _context.Teachers.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return 0;
            if (!teacher.IsDeleted) return -1;

            teacher.IsDeleted = false;
            teacher.DeletedAt = null;
            await _context.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task<bool> IsTeacherClassTeacher(int teacherId)
        {
            return await _context.Grades.AnyAsync(g => g.ClassTeacherId == teacherId);
        }

        public async Task<bool> TeacherExists(int teacherId)
        {
            return await _context.Teachers.AnyAsync(t => t.Id == teacherId);
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

        public async Task<Teacher?> GetTeacherEntityByUserId(int userId)
        {
            return await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<Teacher?> GetTeacherEntityById(int id)
        {
            return await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task LinkUser(int teacherId, int userId)
        {
            var existingWithUser = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (existingWithUser != null && existingWithUser.Id != teacherId)
            {
                existingWithUser.UserId = null;
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher == null)
            {
                return;
            }

            teacher.UserId = userId;
            await _context.SaveChangesAsync();
        }

        public async Task<List<GradeResponseDto>> GetAssignedGrades(int teacherId)
        {
            var classTeacherGrades = await _context.Grades
                .Where(g => g.ClassTeacherId == teacherId)
                .Select(g => new GradeResponseDto
                {
                    Id = g.Id,
                    ClassName = g.ClassName,
                    Level = g.Level,
                    ClassTeacherId = g.ClassTeacherId,
                    ClassTeacher = g.ClassTeacher != null
                        ? new TeacherResponseDto
                        {
                            Id = g.ClassTeacher.Id,
                            Name = g.ClassTeacher.Name,
                            Email = g.ClassTeacher.Email,
                            PhoneNo = g.ClassTeacher.PhoneNo
                        }
                        : null
                })
                .ToListAsync();

            var subjectGrades = await _context.GradeSubjectTeachers
                .Where(gst => gst.TeacherId == teacherId)
                .Select(gst => new GradeResponseDto
                {
                    Id = gst.GradeSubject.GradeId,
                    ClassName = gst.GradeSubject.Grade.ClassName,
                    Level = gst.GradeSubject.Grade.Level,
                    ClassTeacherId = gst.GradeSubject.Grade.ClassTeacherId,
                    ClassTeacher = gst.GradeSubject.Grade.ClassTeacher != null
                        ? new TeacherResponseDto
                        {
                            Id = gst.GradeSubject.Grade.ClassTeacher.Id,
                            Name = gst.GradeSubject.Grade.ClassTeacher.Name,
                            Email = gst.GradeSubject.Grade.ClassTeacher.Email,
                            PhoneNo = gst.GradeSubject.Grade.ClassTeacher.PhoneNo
                        }
                        : null
                })
                .ToListAsync();

            return classTeacherGrades
                .Concat(subjectGrades)
                .GroupBy(g => g.Id)
                .Select(g => g.First())
                .OrderBy(g => g.Id)
                .ToList();
        }

        public async Task<List<StudentDetailsDto>> GetAssignedStudents(int teacherId)
        {
            var gradeIds = (await GetAssignedGrades(teacherId)).Select(g => g.Id).ToList();
            if (gradeIds.Count == 0)
            {
                return new List<StudentDetailsDto>();
            }

            return await _context.Students
                .Where(s => gradeIds.Contains(s.GradeId))
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
                    Subjects = new List<GradeSubjectWithTeachersResponseDto>()
                })
                .OrderBy(s => s.GradeId)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<TeacherSubjectAssignmentDto>> GetAssignedSubjects(int teacherId)
        {
            var assignments = await _context.GradeSubjectTeachers
                .Where(gst => gst.TeacherId == teacherId)
                .Select(gst => new TeacherSubjectAssignmentDto
                {
                    GradeId = gst.GradeSubject.GradeId,
                    GradeName = gst.GradeSubject.Grade.ClassName,
                    SubjectId = gst.GradeSubject.SubjectId,
                    SubjectName = gst.GradeSubject.Subject.Name,
                    IsOptional = gst.GradeSubject.IsOptional
                })
                .ToListAsync();

            return assignments
                .GroupBy(x => new { x.GradeId, x.SubjectId })
                .Select(g => g.First())
                .OrderBy(x => x.GradeName)
                .ThenBy(x => x.SubjectName)
                .ToList();
        }

        public async Task<PagedResult<TeacherResponseDto>> GetTeachersPaged(PaginationParameters parameters)
        {
            var query = _context.Teachers
                .OrderBy(t => t.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                var isNumericSearch = int.TryParse(parameters.Search, out var searchId);
                query = query.Where(t =>
                    t.Name.ToLower().Contains(search) ||
                    t.Email.ToLower().Contains(search) ||
                    (t.PhoneNo != null && t.PhoneNo.ToLower().Contains(search)) ||
                    (isNumericSearch && t.Id == searchId));
            }

            var dtoQuery = query.Select(t => new TeacherResponseDto
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                PhoneNo = t.PhoneNo
            });

            return await dtoQuery.ToPagedResultAsync(parameters);
        }
    }
}

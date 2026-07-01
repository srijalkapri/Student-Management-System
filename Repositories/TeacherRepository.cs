using CRUD.Data;
using CRUD.DTOs;
using CRUD.Extensions;
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

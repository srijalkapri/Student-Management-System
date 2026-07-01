using CRUD.Data;
using CRUD.DTOs;
using CRUD.Extensions;
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
                .Select(g => new GradeResponseDto
                {
                    Id = g.Id,
                    ClassName = g.ClassName,
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
        }

        public async Task<GradeResponseDto?> GetGradeById(int id)
        {
            return await _context.Grades
                .Where(g => g.Id == id)
                .Select(g => new GradeResponseDto
                {
                    Id = g.Id,
                    ClassName = g.ClassName,
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
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<GradeResponseDto>> GetGradesPagedAsync(PaginationParameters parameters)
        {
            var query = _context.Grades
                .OrderBy(g => g.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                var isNumericSearch = int.TryParse(parameters.Search, out var searchId);
                query = query.Where(g =>
                    g.ClassName.ToLower().Contains(search) ||
                    (isNumericSearch && g.Id == searchId));
            }

            var dtoQuery = query.Select(g => new GradeResponseDto
            {
                Id = g.Id,
                ClassName = g.ClassName,
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
            });

            return await dtoQuery.ToPagedResultAsync(parameters);
        }
    }
}

using CRUD.Infrastructure.Persistence;
using CRUD.Application.DTOs;
using CRUD.Application.Extensions;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories
{
    public class GradeSubjectTeacherRepository : IGradeSubjectTeacherRepository
    {
        private readonly ApplicationDbContext _context;

        public GradeSubjectTeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GradeSubjectTeacherResponseDto>> GetAll()
        {
            return await _context.GradeSubjectTeachers
                .Select(gst => new GradeSubjectTeacherResponseDto
                {
                    Id = gst.Id,
                    GradeSubjectId = gst.GradeSubjectId,
                    GradeId = gst.GradeSubject.GradeId,
                    GradeName = gst.GradeSubject.Grade.ClassName,
                    SubjectId = gst.GradeSubject.SubjectId,
                    SubjectName = gst.GradeSubject.Subject.Name,
                    IsOptional = gst.GradeSubject.IsOptional,
                    TeacherId = gst.TeacherId,
                    TeacherName = gst.Teacher.Name
                })
                .ToListAsync();
        }

        public async Task<GradeSubjectTeacherResponseDto?> GetById(int id)
        {
            return await _context.GradeSubjectTeachers
                .Where(gst => gst.Id == id)
                .Select(gst => new GradeSubjectTeacherResponseDto
                {
                    Id = gst.Id,
                    GradeSubjectId = gst.GradeSubjectId,
                    GradeId = gst.GradeSubject.GradeId,
                    GradeName = gst.GradeSubject.Grade.ClassName,
                    SubjectId = gst.GradeSubject.SubjectId,
                    SubjectName = gst.GradeSubject.Subject.Name,
                    IsOptional = gst.GradeSubject.IsOptional,
                    TeacherId = gst.TeacherId,
                    TeacherName = gst.Teacher.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> Create(GradeSubjectTeacherCreateDto gradeSubjectTeacher)
        {
            var entity = new GradeSubjectTeacher
            {
                GradeSubjectId = gradeSubjectTeacher.GradeSubjectId,
                TeacherId = gradeSubjectTeacher.TeacherId
            };
            _context.GradeSubjectTeachers.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> Update(int id, GradeSubjectTeacherCreateDto gradeSubjectTeacher)
        {
            var existing = await _context.GradeSubjectTeachers.FindAsync(id);
            if (existing == null) return 0;

            existing.GradeSubjectId = gradeSubjectTeacher.GradeSubjectId;
            existing.TeacherId = gradeSubjectTeacher.TeacherId;
            await _context.SaveChangesAsync();
            return existing.Id;
        }

        public async Task<int> Delete(int id)
        {
            var gradeSubjectTeacher = await _context.GradeSubjectTeachers.FindAsync(id);
            if (gradeSubjectTeacher == null) return 0;

            _context.GradeSubjectTeachers.Remove(gradeSubjectTeacher);
            await _context.SaveChangesAsync();
            return gradeSubjectTeacher.Id;
        }

        public async Task<bool> GradeSubjectExists(int gradeSubjectId)
        {
            return await _context.GradeSubjects.AnyAsync(gs => gs.Id == gradeSubjectId);
        }

        public async Task<bool> TeacherExists(int teacherId)
        {
            return await _context.Teachers.AnyAsync(t => t.Id == teacherId);
        }

        public async Task<PagedResult<GradeSubjectTeacherResponseDto>> GetPaged(PaginationParameters parameters)
        {
            var query = _context.GradeSubjectTeachers
                .OrderBy(gst => gst.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                var isNumericSearch = int.TryParse(parameters.Search, out var searchId);
                query = query.Where(gst =>
                    gst.Teacher.Name.ToLower().Contains(search) ||
                    gst.GradeSubject.Subject.Name.ToLower().Contains(search) ||
                    gst.GradeSubject.Grade.ClassName.ToLower().Contains(search) ||
                    (isNumericSearch && gst.Id == searchId));
            }

            var dtoQuery = query.Select(gst => new GradeSubjectTeacherResponseDto
            {
                Id = gst.Id,
                GradeSubjectId = gst.GradeSubjectId,
                GradeId = gst.GradeSubject.GradeId,
                GradeName = gst.GradeSubject.Grade.ClassName,
                SubjectId = gst.GradeSubject.SubjectId,
                SubjectName = gst.GradeSubject.Subject.Name,
                IsOptional = gst.GradeSubject.IsOptional,
                TeacherId = gst.TeacherId,
                TeacherName = gst.Teacher.Name
            });

            return await dtoQuery.ToPagedResultAsync(parameters);
        }
    }
}
using CRUD.Data;
using CRUD.DTOs;
using CRUD.Extensions;
using CRUD.Interfaces;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repositories
{
    public class GradeSubjectRepository : IGradeSubjectRepository
    {
        private readonly ApplicationDbContext _context;

        public GradeSubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateGradeSubject(GradeSubject gradeSubject)
        {
            _context.Add(gradeSubject);
            await _context.SaveChangesAsync();
            return gradeSubject.Id;
        }

        public async Task<int> UpdateGradeSubject(int id, GradeSubjectCreateDto gradeSubjectDto)
        {
            var gradeSubject = await _context.GradeSubjects.FindAsync(id);
            if (gradeSubject == null) return 0;

            gradeSubject.GradeId = gradeSubjectDto.GradeId;
            gradeSubject.SubjectId = gradeSubjectDto.SubjectId;
            gradeSubject.IsOptional = gradeSubjectDto.IsOptional;

            await _context.SaveChangesAsync();
            return gradeSubject.Id;
        }

        public async Task<int> DeleteGradeSubject(int id)
        {
            var gradeSubject = await _context.GradeSubjects.FindAsync(id);
            if (gradeSubject == null) return 0;

            _context.GradeSubjects.Remove(gradeSubject);
            await _context.SaveChangesAsync();
            return gradeSubject.Id;
        }

        public async Task<List<GradeSubjectWithTeachersResponseDto>> GetAllGradeSubjects(bool? isOptional = null)
        {
            var query = _context.GradeSubjects.AsQueryable();

            if (isOptional.HasValue)
            {
                query = query.Where(gs => gs.IsOptional == isOptional.Value);
            }

            return await query
                .Select(gs => new GradeSubjectWithTeachersResponseDto
                {
                    Id = gs.Id,
                    GradeId = gs.GradeId,
                    GradeName = gs.Grade.ClassName,
                    SubjectId = gs.SubjectId,
                    SubjectName = gs.Subject.Name,
                    IsOptional = gs.IsOptional,
                    Teachers = gs.GradeSubjectTeachers
                        .Select(gst => new TeacherResponseDto
                        {
                            Id = gst.TeacherId,
                            Name = gst.Teacher.Name
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<GradeSubjectWithTeachersResponseDto?> GetGradeSubjectById(int id)
        {
            return await _context.GradeSubjects
                .Where(gs => gs.Id == id)
                .Select(gs => new GradeSubjectWithTeachersResponseDto
                {
                    Id = gs.Id,
                    GradeId = gs.GradeId,
                    GradeName = gs.Grade.ClassName,
                    SubjectId = gs.SubjectId,
                    SubjectName = gs.Subject.Name,
                    IsOptional = gs.IsOptional,
                    Teachers = gs.GradeSubjectTeachers
                        .Select(gst => new TeacherResponseDto
                        {
                            Id = gst.TeacherId,
                            Name = gst.Teacher.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsByGradeId(int gradeId, bool? isOptional = null)
        {
            var query = _context.GradeSubjects.Where(gs => gs.GradeId == gradeId);

            if (isOptional.HasValue)
            {
                query = query.Where(gs => gs.IsOptional == isOptional.Value);
            }

            return await query
                .Select(gs => new GradeSubjectWithTeachersResponseDto
                {
                    Id = gs.Id,
                    GradeId = gs.GradeId,
                    GradeName = gs.Grade.ClassName,
                    SubjectId = gs.SubjectId,
                    SubjectName = gs.Subject.Name,
                    IsOptional = gs.IsOptional,
                    Teachers = gs.GradeSubjectTeachers
                        .Select(gst => new TeacherResponseDto
                        {
                            Id = gst.TeacherId,
                            Name = gst.Teacher.Name
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<PagedResult<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsPagedAsync(PaginationParameters parameters, bool? isOptional = null)
        {
            var query = _context.GradeSubjects.AsQueryable();

            if (isOptional.HasValue)
            {
                query = query.Where(gs => gs.IsOptional == isOptional.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                var isNumericSearch = int.TryParse(parameters.Search, out var searchId);
                query = query.Where(gs =>
                    gs.Subject.Name.ToLower().Contains(search) ||
                    gs.Grade.ClassName.ToLower().Contains(search) ||
                    (isNumericSearch && gs.Id == searchId));
            }

            var sortBy = string.IsNullOrWhiteSpace(parameters.SortBy) ? "id" : parameters.SortBy.ToLower();
            var sortDirection = string.IsNullOrWhiteSpace(parameters.SortDirection) ? "asc" : parameters.SortDirection.ToLower();

            query = sortBy switch
            {
                "subjectname" => sortDirection == "asc"
                    ? query.OrderBy(gs => gs.Subject.Name)
                    : query.OrderByDescending(gs => gs.Subject.Name),
                "gradename" => sortDirection == "asc"
                    ? query.OrderBy(gs => gs.Grade.ClassName)
                    : query.OrderByDescending(gs => gs.Grade.ClassName),
                _ => sortDirection == "asc"
                    ? query.OrderBy(gs => gs.Id)
                    : query.OrderByDescending(gs => gs.Id)
            };

            var dtoQuery = query.Select(gs => new GradeSubjectWithTeachersResponseDto
            {
                Id = gs.Id,
                GradeId = gs.GradeId,
                GradeName = gs.Grade.ClassName,
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
            });

            return await dtoQuery.ToPagedResultAsync(parameters);
        }

        public async Task<PagedResult<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsByGradeIdPagedAsync(int gradeId, PaginationParameters parameters, bool? isOptional = null)
        {
            var query = _context.GradeSubjects.Where(gs => gs.GradeId == gradeId);

            if (isOptional.HasValue)
            {
                query = query.Where(gs => gs.IsOptional == isOptional.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                var isNumericSearch = int.TryParse(parameters.Search, out var searchId);
                query = query.Where(gs =>
                    gs.Subject.Name.ToLower().Contains(search) ||
                    gs.Grade.ClassName.ToLower().Contains(search) ||
                    (isNumericSearch && gs.Id == searchId));
            }

            var sortBy = string.IsNullOrWhiteSpace(parameters.SortBy) ? "id" : parameters.SortBy.ToLower();
            var sortDirection = string.IsNullOrWhiteSpace(parameters.SortDirection) ? "asc" : parameters.SortDirection.ToLower();

            query = sortBy switch
            {
                "subjectname" => sortDirection == "asc"
                    ? query.OrderBy(gs => gs.Subject.Name)
                    : query.OrderByDescending(gs => gs.Subject.Name),
                "gradename" => sortDirection == "asc"
                    ? query.OrderBy(gs => gs.Grade.ClassName)
                    : query.OrderByDescending(gs => gs.Grade.ClassName),
                _ => sortDirection == "asc"
                    ? query.OrderBy(gs => gs.Id)
                    : query.OrderByDescending(gs => gs.Id)
            };

            var dtoQuery = query.Select(gs => new GradeSubjectWithTeachersResponseDto
            {
                Id = gs.Id,
                GradeId = gs.GradeId,
                GradeName = gs.Grade.ClassName,
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
            });

            return await dtoQuery.ToPagedResultAsync(parameters);
        }

        public async Task<List<GradeSubject>> GetGradeSubjectsByGradeIdEntities(int gradeId)
        {
            return await _context.GradeSubjects
                .Where(gs => gs.GradeId == gradeId)
                .ToListAsync();
        }

        public async Task<GradeSubject?> GetGradeSubjectEntityById(int id)
        {
            return await _context.GradeSubjects.FirstOrDefaultAsync(gs => gs.Id == id);
        }

        public async Task<bool> GradeSubjectExistsForGrade(int gradeSubjectId, int gradeId)
        {
            return await _context.GradeSubjects.AnyAsync(gs => gs.Id == gradeSubjectId && gs.GradeId == gradeId);
        }
    }
}

using CRUD.Data;
using CRUD.DTOs;
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

        public async Task<List<GradeSubjectWithTeachersResponseDto>> GetAllGradeSubjects()
        {
            var gradeSubjects = await _context.GradeSubjects
                .Include(gs => gs.Grade)
                .Include(gs => gs.Subject)
                .ToListAsync();

            var result = new List<GradeSubjectWithTeachersResponseDto>();
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

                result.Add(new GradeSubjectWithTeachersResponseDto
                {
                    Id = gs.Id,
                    GradeId = gs.GradeId,
                    GradeName = gs.Grade.ClassName,
                    SubjectId = gs.SubjectId,
                    SubjectName = gs.Subject.Name,
                    Teachers = teachers
                });
            }
            return result;
        }

        public async Task<GradeSubjectWithTeachersResponseDto?> GetGradeSubjectById(int id)
        {
            var gradeSubject = await _context.GradeSubjects
                .Include(gs => gs.Grade)
                .Include(gs => gs.Subject)
                .FirstOrDefaultAsync(gs => gs.Id == id);

            if (gradeSubject == null) return null;

            var teachers = await _context.GradeSubjectTeachers
                .Include(gst => gst.Teacher)
                .Where(gst => gst.GradeSubjectId == gradeSubject.Id)
                .Select(gst => new TeacherResponseDto
                {
                    Id = gst.TeacherId,
                    Name = gst.Teacher.Name
                })
                .ToListAsync();

            return new GradeSubjectWithTeachersResponseDto
            {
                Id = gradeSubject.Id,
                GradeId = gradeSubject.GradeId,
                GradeName = gradeSubject.Grade.ClassName,
                SubjectId = gradeSubject.SubjectId,
                SubjectName = gradeSubject.Subject.Name,
                Teachers = teachers
            };
        }

        public async Task<List<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsByGradeId(int gradeId)
        {
            var gradeSubjects = await _context.GradeSubjects
                .Include(gs => gs.Grade)
                .Include(gs => gs.Subject)
                .Where(gs => gs.GradeId == gradeId)
                .ToListAsync();

            var result = new List<GradeSubjectWithTeachersResponseDto>();
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

                result.Add(new GradeSubjectWithTeachersResponseDto
                {
                    Id = gs.Id,
                    GradeId = gs.GradeId,
                    GradeName = gs.Grade.ClassName,
                    SubjectId = gs.SubjectId,
                    SubjectName = gs.Subject.Name,
                    Teachers = teachers
                });
            }
            return result;
        }
    }
}

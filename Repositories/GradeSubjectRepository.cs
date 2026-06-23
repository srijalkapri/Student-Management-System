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
            return await _context.GradeSubjects
                .Select(gs => new GradeSubjectWithTeachersResponseDto
                {
                    Id = gs.Id,
                    GradeId = gs.GradeId,
                    GradeName = gs.Grade.ClassName,
                    SubjectId = gs.SubjectId,
                    SubjectName = gs.Subject.Name,
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

        public async Task<List<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsByGradeId(int gradeId)
        {
            return await _context.GradeSubjects
                .Where(gs => gs.GradeId == gradeId)
                .Select(gs => new GradeSubjectWithTeachersResponseDto
                {
                    Id = gs.Id,
                    GradeId = gs.GradeId,
                    GradeName = gs.Grade.ClassName,
                    SubjectId = gs.SubjectId,
                    SubjectName = gs.Subject.Name,
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
    }
}

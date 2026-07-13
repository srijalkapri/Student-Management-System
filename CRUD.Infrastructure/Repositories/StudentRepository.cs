using CRUD.Infrastructure.Persistence;
using CRUD.Application.DTOs;
using CRUD.Application.Extensions;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories
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
            var student = await _context.Students.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return 0;
            if (student.IsDeleted) return -1;

            student.IsDeleted = true;
            student.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<int> RestoreStudent(int id)
        {
            var student = await _context.Students.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return 0;
            if (!student.IsDeleted) return -1;

            student.IsDeleted = false;
            student.DeletedAt = null;
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

        public async Task<Student?> GetStudentEntityByUserId(int userId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<Student?> GetStudentEntityById(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task LinkUser(int studentId, int userId)
        {
            var existingWithUser = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (existingWithUser != null && existingWithUser.Id != studentId)
            {
                existingWithUser.UserId = null;
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
            {
                return;
            }

            student.UserId = userId;
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentDetailsDto>> GetStudentsByGradeId(int gradeId)
        {
            return await _context.Students
                .Where(s => s.GradeId == gradeId)
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

        public async Task<PromoteStudentsResponseDto> PreviewPromotion(int fromGradeId, int toGradeId, List<int>? studentIds)
        {
            // Use LINQ projections to avoid fetching full student entities
            var studentsData = await _context.Students
                .Where(s => s.GradeId == fromGradeId)
                .Where(s => studentIds == null || !studentIds.Any() || studentIds.Contains(s.Id))
                .Select(s => new
                {
                    s.Id,
                    IsAlreadyInTarget = s.GradeId == toGradeId
                })
                .ToListAsync();

            var skippedStudents = studentsData
                .Where(x => x.IsAlreadyInTarget)
                .Select(x => new SkippedStudentDto
                {
                    StudentId = x.Id,
                    Reason = "Already in target grade"
                })
                .ToList();

            return new PromoteStudentsResponseDto
            {
                PromotedCount = studentsData.Count - skippedStudents.Count,
                SkippedCount = skippedStudents.Count,
                SkippedStudents = skippedStudents
            };
        }

        public async Task<PromoteStudentsResponseDto> PromoteStudents(int fromGradeId, int toGradeId, List<int>? studentIds)
        {
            var response = new PromoteStudentsResponseDto();
            var skippedStudents = new List<SkippedStudentDto>();
            var promotedStudentIds = new List<int>();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var studentsQuery = _context.Students.Where(s => s.GradeId == fromGradeId);
                if (studentIds != null && studentIds.Any())
                {
                    studentsQuery = studentsQuery.Where(s => studentIds.Contains(s.Id));
                }

                var students = await studentsQuery.ToListAsync();
                foreach (var student in students)
                {
                    if (student.GradeId == toGradeId)
                    {
                        skippedStudents.Add(new SkippedStudentDto
                        {
                            StudentId = student.Id,
                            Reason = "Already in target grade"
                        });
                        continue;
                    }

                    student.GradeId = toGradeId;
                    promotedStudentIds.Add(student.Id);

                    _context.PromotionHistories.Add(new PromotionHistory
                    {
                        StudentId = student.Id,
                        FromGradeId = fromGradeId,
                        ToGradeId = toGradeId,
                        PromotedAt = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.PromotedCount = promotedStudentIds.Count;
                response.SkippedCount = skippedStudents.Count;
                response.SkippedStudents = skippedStudents;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return response;
        }

        public async Task<PagedResult<StudentDetailsDto>> GetStudentsPaged(PaginationParameters parameters)
        {
            var query = _context.Students
                .OrderBy(s => s.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                var isNumericSearch = int.TryParse(parameters.Search, out var searchId);
                query = query.Where(s =>
                    s.Name.ToLower().Contains(search) ||
                    s.Email.ToLower().Contains(search) ||
                    (s.PhoneNo != null && s.PhoneNo.ToLower().Contains(search)) ||
                    (isNumericSearch && s.Id == searchId));
            }

            var dtoQuery = query.Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                GradeId = s.GradeId,
                GradeName = s.Grade.ClassName
            });

            return await dtoQuery.ToPagedResultAsync(parameters);
        }

        public async Task<PagedResult<StudentDetailsDto>> GetStudentsByGradeIdPaged(int gradeId, PaginationParameters parameters)
        {
            var query = _context.Students
                .Where(s => s.GradeId == gradeId)
                .OrderBy(s => s.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                var isNumericSearch = int.TryParse(parameters.Search, out var searchId);
                query = query.Where(s =>
                    s.Name.ToLower().Contains(search) ||
                    s.Email.ToLower().Contains(search) ||
                    (s.PhoneNo != null && s.PhoneNo.ToLower().Contains(search)) ||
                    (isNumericSearch && s.Id == searchId));
            }

            var dtoQuery = query.Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                PhoneNo = s.PhoneNo,
                GradeId = s.GradeId,
                GradeName = s.Grade.ClassName
            });

            return await dtoQuery.ToPagedResultAsync(parameters);
        }
    }
}

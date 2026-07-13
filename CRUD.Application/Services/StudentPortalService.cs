using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Application.Responses;

namespace CRUD.Application.Services
{
    public class StudentPortalService : IStudentPortalService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentPortalService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<ServiceResponse<StudentPortalOverviewDto>> GetOverview(int userId)
        {
            var response = new ServiceResponse<StudentPortalOverviewDto>();
            var details = await ResolveStudentDetails(userId, response);
            if (details == null)
            {
                return response;
            }

            response.Data = MapOverview(details);
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<StudentProfileDto>> GetMyProfile(int userId)
        {
            var response = new ServiceResponse<StudentProfileDto>();
            var details = await ResolveStudentDetails(userId, response);
            if (details == null)
            {
                return response;
            }

            response.Data = new StudentProfileDto
            {
                Id = details.Id,
                Name = details.Name,
                Email = details.Email,
                PhoneNo = details.PhoneNo,
                GradeId = details.GradeId,
                GradeName = details.GradeName
            };
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<GradeResponseDto>> GetMyGrade(int userId)
        {
            var response = new ServiceResponse<GradeResponseDto>();
            var details = await ResolveStudentDetails(userId, response);
            if (details == null)
            {
                return response;
            }

            response.Data = new GradeResponseDto
            {
                Id = details.GradeId,
                ClassName = details.GradeName,
                ClassTeacherId = details.ClassTeacherId,
                ClassTeacher = details.ClassTeacher
            };
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>> GetMySubjects(int userId)
        {
            var response = new ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>();
            var details = await ResolveStudentDetails(userId, response);
            if (details == null)
            {
                return response;
            }

            response.Data = details.Subjects;
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<List<TeacherResponseDto>>> GetMyTeachers(int userId)
        {
            var response = new ServiceResponse<List<TeacherResponseDto>>();
            var details = await ResolveStudentDetails(userId, response);
            if (details == null)
            {
                return response;
            }

            var teachers = details.Subjects
                .SelectMany(s => s.Teachers)
                .GroupBy(t => t.Id)
                .Select(g => g.First())
                .OrderBy(t => t.Name)
                .ToList();

            if (details.ClassTeacher != null && teachers.All(t => t.Id != details.ClassTeacher.Id))
            {
                teachers.Insert(0, details.ClassTeacher);
            }

            response.Data = teachers;
            response.Message = "Success";
            return response;
        }

        private async Task<StudentDetailsDto?> ResolveStudentDetails<T>(int userId, ServiceResponse<T> response)
        {
            var student = await _studentRepository.GetStudentEntityByUserId(userId);
            if (student == null)
            {
                response.Success = false;
                response.Message = "No student profile is linked to this user account.";
                return null;
            }

            var details = await _studentRepository.GetStudentById(student.Id);
            if (details == null)
            {
                response.Success = false;
                response.Message = "Student profile not found.";
            }

            return details;
        }

        private static StudentPortalOverviewDto MapOverview(StudentDetailsDto details)
        {
            var teachers = details.Subjects
                .SelectMany(s => s.Teachers)
                .GroupBy(t => t.Id)
                .Select(g => g.First())
                .OrderBy(t => t.Name)
                .ToList();

            if (details.ClassTeacher != null && teachers.All(t => t.Id != details.ClassTeacher.Id))
            {
                teachers.Insert(0, details.ClassTeacher);
            }

            return new StudentPortalOverviewDto
            {
                Profile = new StudentProfileDto
                {
                    Id = details.Id,
                    Name = details.Name,
                    Email = details.Email,
                    PhoneNo = details.PhoneNo,
                    GradeId = details.GradeId,
                    GradeName = details.GradeName
                },
                Grade = new GradeResponseDto
                {
                    Id = details.GradeId,
                    ClassName = details.GradeName,
                    ClassTeacherId = details.ClassTeacherId,
                    ClassTeacher = details.ClassTeacher
                },
                Subjects = details.Subjects,
                Teachers = teachers
            };
        }
    }
}

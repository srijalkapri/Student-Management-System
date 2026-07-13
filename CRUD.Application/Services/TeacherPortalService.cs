using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Application.Responses;

namespace CRUD.Application.Services
{
    public class TeacherPortalService : ITeacherPortalService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherPortalService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<ServiceResponse<TeacherPortalOverviewDto>> GetOverview(int userId)
        {
            var response = new ServiceResponse<TeacherPortalOverviewDto>();
            var teacher = await ResolveTeacher(userId, response);
            if (teacher == null)
            {
                return response;
            }

            var profile = await _teacherRepository.GetTeacherById(teacher.Id);
            response.Data = new TeacherPortalOverviewDto
            {
                Profile = profile!,
                Classes = await _teacherRepository.GetAssignedGrades(teacher.Id),
                Students = await _teacherRepository.GetAssignedStudents(teacher.Id),
                Subjects = await _teacherRepository.GetAssignedSubjects(teacher.Id)
            };
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<TeacherResponseDto>> GetMyProfile(int userId)
        {
            var response = new ServiceResponse<TeacherResponseDto>();
            var teacher = await ResolveTeacher(userId, response);
            if (teacher == null)
            {
                return response;
            }

            response.Data = await _teacherRepository.GetTeacherById(teacher.Id);
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<List<GradeResponseDto>>> GetMyClasses(int userId)
        {
            var response = new ServiceResponse<List<GradeResponseDto>>();
            var teacher = await ResolveTeacher(userId, response);
            if (teacher == null)
            {
                return response;
            }

            response.Data = await _teacherRepository.GetAssignedGrades(teacher.Id);
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<List<StudentDetailsDto>>> GetMyStudents(int userId)
        {
            var response = new ServiceResponse<List<StudentDetailsDto>>();
            var teacher = await ResolveTeacher(userId, response);
            if (teacher == null)
            {
                return response;
            }

            response.Data = await _teacherRepository.GetAssignedStudents(teacher.Id);
            response.Message = "Success";
            return response;
        }

        public async Task<ServiceResponse<List<TeacherSubjectAssignmentDto>>> GetMySubjects(int userId)
        {
            var response = new ServiceResponse<List<TeacherSubjectAssignmentDto>>();
            var teacher = await ResolveTeacher(userId, response);
            if (teacher == null)
            {
                return response;
            }

            response.Data = await _teacherRepository.GetAssignedSubjects(teacher.Id);
            response.Message = "Success";
            return response;
        }

        private async Task<Domain.Models.Teacher?> ResolveTeacher<T>(int userId, ServiceResponse<T> response)
        {
            var teacher = await _teacherRepository.GetTeacherEntityByUserId(userId);
            if (teacher == null)
            {
                response.Success = false;
                response.Message = "No teacher profile is linked to this user account.";
            }

            return teacher;
        }
    }
}

using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Responses;
using System.Threading.Tasks;

namespace CRUD.Services
{
    public class GradeSubjectTeacherService : IGradeSubjectTeacherService
    {
        private readonly IGradeSubjectTeacherRepository _repository;

        public GradeSubjectTeacherService(IGradeSubjectTeacherRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<int>> CreateGradeSubjectTeacher(GradeSubjectTeacherCreateDto dto)
        {
            var response = new ServiceResponse<int>();
            
            if (!await _repository.GradeSubjectExists(dto.GradeSubjectId))
            {
                response.Success = false;
                response.Message = "GradeSubject not found.";
                return response;
            }
            
            if (!await _repository.TeacherExists(dto.TeacherId))
            {
                response.Success = false;
                response.Message = "Teacher not found.";
                return response;
            }

            var result = await _repository.Create(dto);
            response.Data = result;
            response.Message = "GradeSubjectTeacher created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateGradeSubjectTeacher(int id, GradeSubjectTeacherCreateDto dto)
        {
            var response = new ServiceResponse<int>();

            if (!await _repository.GradeSubjectExists(dto.GradeSubjectId))
            {
                response.Success = false;
                response.Message = "GradeSubject not found.";
                return response;
            }
            
            if (!await _repository.TeacherExists(dto.TeacherId))
            {
                response.Success = false;
                response.Message = "Teacher not found.";
                return response;
            }

            var result = await _repository.Update(id, dto);
            if (result == 0)
            {
                response.Success = false;
                response.Message = "GradeSubjectTeacher not found.";
                return response;
            }

            response.Data = result;
            response.Message = "GradeSubjectTeacher updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteGradeSubjectTeacher(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _repository.Delete(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = "GradeSubjectTeacher not found.";
                return response;
            }

            response.Data = id;
            response.Message = "GradeSubjectTeacher deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<GradeSubjectTeacherResponseDto>>> GetAllGradeSubjectTeachers()
        {
            var response = new ServiceResponse<List<GradeSubjectTeacherResponseDto>>();
            var gradeSubjectTeachers = await _repository.GetAll();
            response.Data = gradeSubjectTeachers;
            response.Message = "All GradeSubjectTeachers retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<GradeSubjectTeacherResponseDto>> GetGradeSubjectTeacherById(int id)
        {
            var response = new ServiceResponse<GradeSubjectTeacherResponseDto>();
            var gradeSubjectTeacher = await _repository.GetById(id);
            if (gradeSubjectTeacher == null)
            {
                response.Success = false;
                response.Message = "GradeSubjectTeacher not found.";
                return response;
            }

            response.Data = gradeSubjectTeacher;
            response.Message = "GradeSubjectTeacher retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<GradeSubjectTeacherResponseDto>>> GetGradeSubjectTeachersPagedAsync(PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<GradeSubjectTeacherResponseDto>>();

            var pagedResult = await _repository.GetPagedAsync(parameters);

            response.Data = pagedResult;
            response.Message = "GradeSubjectTeachers retrieved successfully.";
            return response;
        }
    }
}

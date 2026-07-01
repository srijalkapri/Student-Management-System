using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using CRUD.Responses;

namespace CRUD.Services
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public async Task<ServiceResponse<int>> CreateGrade(GradeCreateDto gradeDto)
        {
            var response = new ServiceResponse<int>();
            var grade = new Grade
            {
                ClassName = gradeDto.ClassName,
                ClassTeacherId = gradeDto.ClassTeacherId
            };

            var gradeId = await _gradeRepository.CreateGrade(grade);
            response.Data = gradeId;
            response.Message = "Grade created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateGrade(int id, GradeCreateDto gradeDto)
        {
            var response = new ServiceResponse<int>();
            var grade = new Grade
            {
                Id = id,
                ClassName = gradeDto.ClassName,
                ClassTeacherId = gradeDto.ClassTeacherId
            };

            var result = await _gradeRepository.UpdateGrade(grade);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Grade with ID {id} not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Grade updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteGrade(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _gradeRepository.DeleteGrade(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Grade with ID {id} not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Grade deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<GradeResponseDto>>> GetAllGrades()
        {
            var response = new ServiceResponse<List<GradeResponseDto>>();
            var grades = await _gradeRepository.GetAllGrades();
            response.Data = grades;
            response.Message = "All grades retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<GradeResponseDto?>> GetGradeById(int id)
        {
            var response = new ServiceResponse<GradeResponseDto?>();
            var grade = await _gradeRepository.GetGradeById(id);
            if (grade == null)
            {
                response.Success = false;
                response.Message = $"Grade with ID {id} not found.";
                return response;
            }

            response.Data = grade;
            response.Message = "Grade retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<GradeResponseDto>>> GetGradesPaged(PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<GradeResponseDto>>();

            var pagedResult = await _gradeRepository.GetGradesPaged(parameters);

            response.Data = pagedResult;
            response.Message = "Grades retrieved successfully.";
            return response;
        }
    }
}

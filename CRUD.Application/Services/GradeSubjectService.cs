using CRUD.Application.DTOs;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using CRUD.Application.Responses;

namespace CRUD.Application.Services
{
    public class GradeSubjectService : IGradeSubjectService
    {
        private readonly IGradeSubjectRepository _gradeSubjectRepository;
        private readonly IGradeRepository _gradeRepository;

        public GradeSubjectService(IGradeSubjectRepository gradeSubjectRepository, IGradeRepository gradeRepository)
        {
            _gradeSubjectRepository = gradeSubjectRepository;
            _gradeRepository = gradeRepository;
        }

        public async Task<ServiceResponse<int>> CreateGradeSubject(GradeSubjectCreateDto gradeSubjectDto)
        {
            var response = new ServiceResponse<int>();
            var gradeSubject = new GradeSubject
            {
                GradeId = gradeSubjectDto.GradeId,
                SubjectId = gradeSubjectDto.SubjectId,
                IsOptional = gradeSubjectDto.IsOptional
            };

            var gradeSubjectId = await _gradeSubjectRepository.CreateGradeSubject(gradeSubject);
            response.Data = gradeSubjectId;
            response.Message = "Grade subject created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateGradeSubject(int id, GradeSubjectCreateDto gradeSubjectDto)
        {
            var response = new ServiceResponse<int>();
            var result = await _gradeSubjectRepository.UpdateGradeSubject(id, gradeSubjectDto);
            if (result == 0)
            {
                response.Success = false;
                response.Message = "Grade subject not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Grade subject updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteGradeSubject(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _gradeSubjectRepository.DeleteGradeSubject(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = "Grade subject not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Grade subject deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>> GetAllGradeSubjects(bool? isOptional = null)
        {
            var response = new ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>();
            var gradeSubjects = await _gradeSubjectRepository.GetAllGradeSubjects(isOptional);
            response.Data = gradeSubjects;
            response.Message = "All grade subjects retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<GradeSubjectWithTeachersResponseDto?>> GetGradeSubjectById(int id)
        {
            var response = new ServiceResponse<GradeSubjectWithTeachersResponseDto?>();
            var gradeSubject = await _gradeSubjectRepository.GetGradeSubjectById(id);
            if (gradeSubject == null)
            {
                response.Success = false;
                response.Message = "Grade subject not found.";
                return response;
            }

            response.Data = gradeSubject;
            response.Message = "Grade subject retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>> GetGradeSubjectsByGradeId(int gradeId, bool? isOptional = null)
        {
            var response = new ServiceResponse<List<GradeSubjectWithTeachersResponseDto>>();
            var gradeSubjects = await _gradeSubjectRepository.GetGradeSubjectsByGradeId(gradeId, isOptional);
            response.Data = gradeSubjects;
            response.Message = "Grade subjects by grade retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<GradeSubjectWithTeachersResponseDto>>> GetGradeSubjectsPaged(PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<GradeSubjectWithTeachersResponseDto>>();

            var pagedResult = await _gradeSubjectRepository.GetGradeSubjectsPaged(parameters);

            response.Data = pagedResult;
            response.Message = "Grade subjects retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<GradeSubjectWithTeachersResponseDto>>> GetGradeSubjectsByGradeIdPaged(int gradeId, PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<GradeSubjectWithTeachersResponseDto>>();

            var grade = await _gradeRepository.GetGradeById(gradeId);
            if (grade == null)
            {
                response.Success = false;
                response.Message = $"Grade with ID {gradeId} not found.";
                return response;
            }

            var pagedResult = await _gradeSubjectRepository.GetGradeSubjectsByGradeIdPaged(gradeId, parameters);

            response.Data = pagedResult;
            response.Message = $"Grade subjects for grade {gradeId} retrieved successfully.";
            return response;
        }
    }
}

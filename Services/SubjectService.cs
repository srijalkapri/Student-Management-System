using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using CRUD.Responses;

namespace CRUD.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<ServiceResponse<int>> CreateSubject(SubjectCreateDto subjectDto)
        {
            var response = new ServiceResponse<int>();
            var subject = new Subject
            {
                Name = subjectDto.Name
            };

            var subjectId = await _subjectRepository.CreateSubject(subject);
            response.Data = subjectId;
            response.Message = "Subject created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateSubject(int id, SubjectCreateDto subjectDto)
        {
            var response = new ServiceResponse<int>();
            var subject = new Subject
            {
                Id = id,
                Name = subjectDto.Name
            };

            var result = await _subjectRepository.UpdateSubject(subject);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Subject with ID {id} not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Subject updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteSubject(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _subjectRepository.DeleteSubject(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Subject with ID {id} not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Subject deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<SubjectResponseDto>>> GetAllSubjects()
        {
            var response = new ServiceResponse<List<SubjectResponseDto>>();
            var subjects = await _subjectRepository.GetAllSubjects();
            response.Data = subjects;
            response.Message = "All subjects retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<SubjectResponseDto?>> GetSubjectById(int id)
        {
            var response = new ServiceResponse<SubjectResponseDto?>();
            var subject = await _subjectRepository.GetSubjectById(id);
            if (subject == null)
            {
                response.Success = false;
                response.Message = $"Subject with ID {id} not found.";
                return response;
            }

            response.Data = subject;
            response.Message = "Subject retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<SubjectResponseDto>>> GetSubjectsPaged(PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<SubjectResponseDto>>();

            var pagedResult = await _subjectRepository.GetSubjectsPaged(parameters);

            response.Data = pagedResult;
            response.Message = "Subjects retrieved successfully.";
            return response;
        }
    }
}

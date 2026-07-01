using CRUD.Interfaces;
using CRUD.Models;
using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGradeRepository _gradeRepository;

        public StudentService(IStudentRepository studentRepository, IGradeRepository gradeRepository)
        {
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
        }

        public async Task<ServiceResponse<int>> CreateStudent(StudentCreateDto studentDto)
        {
            var response = new ServiceResponse<int>();
            var student = new Student
            {
                Name = studentDto.Name,
                Email = studentDto.Email,
                PhoneNo = studentDto.PhoneNo,
                GradeId = studentDto.GradeId
            };

            var studentId = await _studentRepository.CreateStudent(student);
            response.Data = studentId;
            response.Message = "Student created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateStudent(int id, StudentCreateDto studentDto)
        {
            var response = new ServiceResponse<int>();
            var result = await _studentRepository.UpdateStudent(id, studentDto.Name, studentDto.Email, studentDto.PhoneNo, studentDto.GradeId);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Student with ID {id} not found.";
                return response;
            }

            response.Data = result;
            response.Message = "Student updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteStudent(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _studentRepository.DeleteStudent(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Student with ID {id} not found.";
                return response;
            }
            if (result == -1)
            {
                response.Success = false;
                response.Message = $"Student with ID {id} is already deleted.";
                return response;
            }

            response.Data = result;
            response.Message = "Student deleted successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> RestoreStudent(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _studentRepository.RestoreStudent(id);
            if (result == 0)
            {
                response.Success = false;
                response.Message = $"Student with ID {id} not found.";
                return response;
            }
            if (result == -1)
            {
                response.Success = false;
                response.Message = $"Student with ID {id} is not deleted.";
                return response;
            }

            response.Data = result;
            response.Message = "Student restored successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<StudentDetailsDto>>> GetAllStudents()
        {
            var response = new ServiceResponse<List<StudentDetailsDto>>();
            var students = await _studentRepository.GetAllStudents();
            response.Data = students;
            response.Message = "All students retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<StudentDetailsDto?>> GetStudentById(int id)
        {
            var response = new ServiceResponse<StudentDetailsDto?>();
            var student = await _studentRepository.GetStudentById(id);
            if (student == null)
            {
                response.Success = false;
                response.Message = $"Student with ID {id} not found.";
                return response;
            }

            response.Data = student;
            response.Message = "Student retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<StudentDetailsDto>>> GetStudentsByGradeId(int gradeId)
        {
            var response = new ServiceResponse<List<StudentDetailsDto>>();
            var grade = await _gradeRepository.GetGradeById(gradeId);
            if (grade == null)
            {
                response.Success = false;
                response.Message = $"Grade with ID {gradeId} not found.";
                return response;
            }

            var students = await _studentRepository.GetStudentsByGradeId(gradeId);
            response.Data = students;
            response.Message = $"Students in grade {gradeId} retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<PromoteStudentsResponseDto>> PreviewPromotion(PromoteStudentsRequestDto request)
        {
            var response = new ServiceResponse<PromoteStudentsResponseDto>();

            // Validate grades
            var fromGrade = await _gradeRepository.GetGradeById(request.FromGradeId);
            if (fromGrade == null)
            {
                response.Success = false;
                response.Message = $"Source grade with ID {request.FromGradeId} not found.";
                return response;
            }

            var toGrade = await _gradeRepository.GetGradeById(request.ToGradeId);
            if (toGrade == null)
            {
                response.Success = false;
                response.Message = $"Target grade with ID {request.ToGradeId} not found.";
                return response;
            }

            if (request.FromGradeId == request.ToGradeId)
            {
                response.Success = false;
                response.Message = "Source and target grades must be different.";
                return response;
            }

            var result = await _studentRepository.PreviewPromotion(request.FromGradeId, request.ToGradeId, request.StudentIds);
            response.Data = result;
            response.Message = "Promotion preview generated successfully.";
            return response;
        }

        public async Task<ServiceResponse<PromoteStudentsResponseDto>> PromoteStudents(PromoteStudentsRequestDto request)
        {
            var response = new ServiceResponse<PromoteStudentsResponseDto>();

            // Validate grades
            var fromGrade = await _gradeRepository.GetGradeById(request.FromGradeId);
            if (fromGrade == null)
            {
                response.Success = false;
                response.Message = $"Source grade with ID {request.FromGradeId} not found.";
                return response;
            }

            var toGrade = await _gradeRepository.GetGradeById(request.ToGradeId);
            if (toGrade == null)
            {
                response.Success = false;
                response.Message = $"Target grade with ID {request.ToGradeId} not found.";
                return response;
            }

            if (request.FromGradeId == request.ToGradeId)
            {
                response.Success = false;
                response.Message = "Source and target grades must be different.";
                return response;
            }

            var result = await _studentRepository.PromoteStudents(request.FromGradeId, request.ToGradeId, request.StudentIds);
            response.Data = result;
            response.Message = $"{result.PromotedCount} students promoted successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<StudentDetailsDto>>> GetStudentsPaged(PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<StudentDetailsDto>>();

            var pagedResult = await _studentRepository.GetStudentsPaged(parameters);

            response.Data = pagedResult;
            response.Message = "Students retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<PagedResult<StudentDetailsDto>>> GetStudentsByGradeIdPaged(int gradeId, PaginationParameters parameters)
        {
            var response = new ServiceResponse<PagedResult<StudentDetailsDto>>();

            // Check if grade exists
            var grade = await _gradeRepository.GetGradeById(gradeId);
            if (grade == null)
            {
                response.Success = false;
                response.Message = $"Grade with ID {gradeId} not found.";
                return response;
            }

            var pagedResult = await _studentRepository.GetStudentsByGradeIdPaged(gradeId, parameters);

            response.Data = pagedResult;
            response.Message = $"Students in grade {gradeId} retrieved successfully.";
            return response;
        }
    }
}

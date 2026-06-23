using CRUD.Interfaces;
using CRUD.Models;
using CRUD.DTOs;
using CRUD.Responses;

namespace CRUD.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<ServiceResponse<int>> CreateStudent(StudentCreateDto studentDto)
        {
            var response = new ServiceResponse<int>();
            var student = new Student
            {
                Name = studentDto.Name,
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
            var result = await _studentRepository.UpdateStudent(id, studentDto.Name, studentDto.GradeId);
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

            response.Data = result;
            response.Message = "Student deleted successfully.";
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
    }
}

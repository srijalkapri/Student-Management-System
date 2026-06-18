using CRUD.Interfaces;
using CRUD.Models;
using CRUD.DTOs;
using CRUD.Repositories;



namespace CRUD.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentRepository _studentRepository;

        public StudentService(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;

        }

        public async Task<int> CreateStudent(StudentCreate studentDto)
        {

            var student = new Student
            {
                Name = studentDto.Name,
                Grade = studentDto.Grade,
                TeacherId = studentDto.TeacherId
            };

            return await _studentRepository.CreateStudent(student);
        }

        public async Task<int> UpdateStudent(int id, StudentCreate studentDto)
        {
            var student = new Student
            {
                Id = id,
                Name = studentDto.Name,
                Grade = studentDto.Grade,
                TeacherId = studentDto.TeacherId
            };
            return await _studentRepository.UpdateStudent(student);
        }

        public async Task<int> DeleteStudent (int id)
        {
            return await _studentRepository.DeleteStudent(id);
        }

        public async Task<List<StudentDetailsDto>> GetAllStudents() {

           return await _studentRepository.GetAllStudents();
        }


        public async Task<StudentDetailsDto?> GetStudentById(int id)
        {
            return await _studentRepository.GetStudentById(id);
        }

    }
      
}

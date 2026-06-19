using CRUD.Interfaces;
using CRUD.Models;
using CRUD.DTOs;
using CRUD.Repositories;




namespace CRUD.Services 
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
            
        public StudentService(IStudentRepository studentRepository)
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
            var students = await _studentRepository.GetAllStudents();

            var existingStudent = students.FirstOrDefault(s => s.Id == id);



            if (existingStudent == null) {

                return 0;
            }


            existingStudent.Name = studentDto.Name;
            existingStudent.Grade = studentDto.Grade;
            existingStudent.TeacherId = studentDto.TeacherId;

             await _studentRepository.UpdateStudent(existingStudent);
            return existingStudent.Id;

        }

        public async Task<int> DeleteStudent (int id)
        {
            return await _studentRepository.DeleteStudent(id);
        }

        public async Task<List<StudentDetailsDto>> GetAllStudents() {


            var students = await _studentRepository.GetAllStudents();

            return students.Select(s => new StudentDetailsDto
            {
                StudentId = s.Id,
                StudentName = s.Name,
                StudentGrade = s.Grade,
                TeacherName = s.Teacher != null ? s.Teacher.Name : "No Teacher Assigned",
                TeacherSubject = s.Teacher != null ? s.Teacher.Subject : "N/A"
            }).ToList();
        }


        public async Task<StudentDetailsDto?> GetStudentById(int id)
        {

            var student = await _studentRepository.GetRawStudentById(id);

            if (student == null)
            {
                return null;
            }

            return new StudentDetailsDto
            {
                StudentId = student.Id,
                StudentName = student.Name,
                StudentGrade = student.Grade,
                TeacherName = student.Teacher != null ? student.Teacher.Name : "No Teacher Assigned",
                TeacherSubject = student.Teacher != null ? student.Teacher.Subject : "N/A"
            };
        }

    }
      
}

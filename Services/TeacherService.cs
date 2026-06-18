using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using CRUD.Repositories;


namespace CRUD.Services
{

    public class TeacherService : ITeacherServices
    {
        private readonly TeacherRepository _teacherRepository;
      
        public TeacherService(TeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

     public async Task<int>CreateTeacher(TeacherCreateDto teacherdto)
        {
            var teacher = new Teacher { 

            Name= teacherdto.Name,
            Subject= teacherdto.Subject,
            Grades= teacherdto.Grades
               
            };
            return await _teacherRepository.CreateTeacher(teacher);
        }
   
        public async Task<int>UpdateTeacher(int id, TeacherCreateDto teacherdto) {

            var teacher = new Teacher { 
            
            Id= id,
            Name = teacherdto.Name,
            Subject= teacherdto.Subject,
            Grades = teacherdto.Grades
            
        };
            return await _teacherRepository.UpdateTeacher(teacher);
        }

        public async Task<int>DeleteTeacher(int id)
        {
            return await _teacherRepository.DeleteTeacher(id);
        }

        public async Task<List<TeacherResponseDto>> GetAllTeachers()
        {
            return await _teacherRepository.GetAllTeachers();

        }

        public async Task<TeacherResponseDto?> GetTeacherById(int id)
        {
            return await _teacherRepository.GetTeacherById(id);
        }

        public async Task<TeacherDetailDto?> GetTeacherDetails(int id)
        {
            return await _teacherRepository.GetTeacherDetails(id);
        }


    }
}

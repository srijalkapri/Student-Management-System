using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using CRUD.Responses;
using CRUD.Repositories;
using System.Linq;
using System.Collections.Generic;
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
            
            if (!await _repository.GradeSubjectExistsAsync(dto.GradeSubjectId))
            {
                response.Success = false;
                response.Message = "GradeSubject not found.";
                return response;
            }
            
            if (!await _repository.TeacherExistsAsync(dto.TeacherId))
            {
                response.Success = false;
                response.Message = "Teacher not found.";
                return response;
            }

            var gradeSubjectTeacher = new GradeSubjectTeacher
            {
                GradeSubjectId = dto.GradeSubjectId,
                TeacherId = dto.TeacherId
            };

            var result = await _repository.CreateAsync(gradeSubjectTeacher);
            response.Data = result.Id;
            response.Message = "GradeSubjectTeacher created successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> UpdateGradeSubjectTeacher(int id, GradeSubjectTeacherCreateDto dto)
        {
            var response = new ServiceResponse<int>();

            if (!await _repository.GradeSubjectExistsAsync(dto.GradeSubjectId))
            {
                response.Success = false;
                response.Message = "GradeSubject not found.";
                return response;
            }
            
            if (!await _repository.TeacherExistsAsync(dto.TeacherId))
            {
                response.Success = false;
                response.Message = "Teacher not found.";
                return response;
            }

            var gradeSubjectTeacher = new GradeSubjectTeacher
            {
                Id = id,
                GradeSubjectId = dto.GradeSubjectId,
                TeacherId = dto.TeacherId
            };

            var result = await _repository.UpdateAsync(gradeSubjectTeacher);
            if (result == null)
            {
                response.Success = false;
                response.Message = "GradeSubjectTeacher not found.";
                return response;
            }

            response.Data = result.Id;
            response.Message = "GradeSubjectTeacher updated successfully.";
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteGradeSubjectTeacher(int id)
        {
            var response = new ServiceResponse<int>();
            var result = await _repository.DeleteAsync(id);
            if (!result)
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
            var gradeSubjectTeachers = await _repository.GetAllAsync();
            var dtos = gradeSubjectTeachers.Select(gst => new GradeSubjectTeacherResponseDto
            {
                Id = gst.Id,
                GradeSubjectId = gst.GradeSubjectId,
                GradeId = gst.GradeSubject.GradeId,
                GradeName = gst.GradeSubject.Grade.ClassName,
                SubjectId = gst.GradeSubject.SubjectId,
                SubjectName = gst.GradeSubject.Subject.Name,
                TeacherId = gst.TeacherId,
                TeacherName = gst.Teacher.Name
            }).ToList();

            response.Data = dtos;
            response.Message = "All GradeSubjectTeachers retrieved successfully.";
            return response;
        }

        public async Task<ServiceResponse<GradeSubjectTeacherResponseDto>> GetGradeSubjectTeacherById(int id)
        {
            var response = new ServiceResponse<GradeSubjectTeacherResponseDto>();
            var gradeSubjectTeacher = await _repository.GetByIdAsync(id);
            if (gradeSubjectTeacher == null)
            {
                response.Success = false;
                response.Message = "GradeSubjectTeacher not found.";
                return response;
            }

            var dto = new GradeSubjectTeacherResponseDto
            {
                Id = gradeSubjectTeacher.Id,
                GradeSubjectId = gradeSubjectTeacher.GradeSubjectId,
                GradeId = gradeSubjectTeacher.GradeSubject.GradeId,
                GradeName = gradeSubjectTeacher.GradeSubject.Grade.ClassName,
                SubjectId = gradeSubjectTeacher.GradeSubject.SubjectId,
                SubjectName = gradeSubjectTeacher.GradeSubject.Subject.Name,
                TeacherId = gradeSubjectTeacher.TeacherId,
                TeacherName = gradeSubjectTeacher.Teacher.Name
            };

            response.Data = dto;
            response.Message = "GradeSubjectTeacher retrieved successfully.";
            return response;
        }
    }
}

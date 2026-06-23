using CRUD.DTOs;
using CRUD.Models;

namespace CRUD.Interfaces
{
    public interface IGradeSubjectRepository
    {
        Task<int> CreateGradeSubject(GradeSubject gradeSubject);
        Task<int> UpdateGradeSubject(int id, GradeSubjectCreateDto gradeSubjectDto);
        Task<int> DeleteGradeSubject(int id);
        Task<List<GradeSubjectWithTeachersResponseDto>> GetAllGradeSubjects();
        Task<GradeSubjectWithTeachersResponseDto?> GetGradeSubjectById(int id);
        Task<List<GradeSubjectWithTeachersResponseDto>> GetGradeSubjectsByGradeId(int gradeId);
    }
}

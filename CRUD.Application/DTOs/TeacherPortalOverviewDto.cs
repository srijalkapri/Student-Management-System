namespace CRUD.Application.DTOs
{
    public class TeacherPortalOverviewDto
    {
        public TeacherResponseDto Profile { get; set; } = new();
        public List<GradeResponseDto> Classes { get; set; } = new();
        public List<StudentDetailsDto> Students { get; set; } = new();
        public List<TeacherSubjectAssignmentDto> Subjects { get; set; } = new();
    }
}

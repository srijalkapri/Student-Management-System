namespace CRUD.Application.DTOs
{
    public class StudentPortalOverviewDto
    {
        public StudentProfileDto Profile { get; set; } = new();
        public GradeResponseDto? Grade { get; set; }
        public List<GradeSubjectWithTeachersResponseDto> Subjects { get; set; } = new();
        public List<TeacherResponseDto> Teachers { get; set; } = new();
    }

    public class StudentProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
    }
}

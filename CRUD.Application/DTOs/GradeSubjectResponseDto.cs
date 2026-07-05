namespace CRUD.Application.DTOs
{
    public class GradeSubjectResponseDto
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public bool IsOptional { get; set; }
    }

    public class GradeSubjectWithTeachersResponseDto
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public bool IsOptional { get; set; }
        public List<TeacherResponseDto> Teachers { get; set; } = new List<TeacherResponseDto>();
    }
}

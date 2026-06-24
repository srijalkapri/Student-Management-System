namespace CRUD.DTOs
{
    public class GradeSubjectTeacherResponseDto
    {
        public int Id { get; set; }
        public int GradeSubjectId { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public bool IsOptional { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
    }
}
namespace CRUD.Application.DTOs
{
    public class TeacherSubjectAssignmentDto
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public bool IsOptional { get; set; }
    }
}

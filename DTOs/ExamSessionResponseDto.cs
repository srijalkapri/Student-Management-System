namespace CRUD.DTOs
{
    public class ExamSessionResponseDto
    {
        public int Id { get; set; }
        public int GradeSubjectId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public bool IsOptional { get; set; }
        public DateTime? ExamDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? InvigilatorTeacherId { get; set; }
        public string? InvigilatorTeacherName { get; set; }
        public string? Notes { get; set; }
    }
}

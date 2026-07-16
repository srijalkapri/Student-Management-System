namespace CRUD.Application.DTOs
{
    public class TeacherExamSessionDto
    {
        public int ExamSessionId { get; set; }
        public int ExamScheduleId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;
        public string? AcademicYear { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int GradeSubjectId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public DateTime? ExamDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? BatchId { get; set; }
        public string? ResultStatus { get; set; }
    }
}

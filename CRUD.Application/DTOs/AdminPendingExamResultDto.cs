namespace CRUD.Application.DTOs
{
    public class AdminPendingExamResultDto
    {
        public int BatchId { get; set; }
        public int ExamSessionId { get; set; }
        public int ExamScheduleId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public DateTime? SubmittedAtUtc { get; set; }
    }
}

namespace CRUD.Application.DTOs
{
    public class AdminExamResultReviewDto
    {
        public int BatchId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ExamTitle { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateTime? SubmittedAtUtc { get; set; }
        public string? ReviewComment { get; set; }
        public List<TeacherExamResultItemDto> Items { get; set; } = new();
    }
}

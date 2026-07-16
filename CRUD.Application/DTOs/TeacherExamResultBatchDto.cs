namespace CRUD.Application.DTOs
{
    public class TeacherExamResultBatchDto
    {
        public int BatchId { get; set; }
        public int ExamSessionId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? SubmittedAtUtc { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }
        public string? ReviewComment { get; set; }
        public List<TeacherExamResultItemDto> Items { get; set; } = new();
    }
}

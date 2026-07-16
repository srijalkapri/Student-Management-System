namespace CRUD.Domain.Models
{
    public class ExamResultBatch
    {
        public int Id { get; set; }
        public int ExamSessionId { get; set; }
        public int TeacherId { get; set; }
        public ExamResultStatus Status { get; set; } = ExamResultStatus.Draft;
        public DateTime? SubmittedAtUtc { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }
        public int? ReviewedByUserId { get; set; }
        public string? ReviewComment { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public virtual ExamSession ExamSession { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual User? ReviewedByUser { get; set; }
        public virtual ICollection<ExamResultItem> Items { get; set; } = new List<ExamResultItem>();
    }
}

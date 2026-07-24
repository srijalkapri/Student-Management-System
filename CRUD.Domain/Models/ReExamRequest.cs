namespace CRUD.Domain.Models
{
    public class ReExamRequest
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ExamSessionId { get; set; }
        public int OriginalResultItemId { get; set; }
        public int AttemptNumber { get; set; } = 1;
        public string? StudentReason { get; set; }
        public ReExamRequestStatus Status { get; set; } = ReExamRequestStatus.Requested;

        public int? ReviewedByUserId { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }
        public string? AdminComment { get; set; }

        public int? TeacherId { get; set; }
        public decimal? MarksObtained { get; set; }
        public decimal? TotalMarks { get; set; }
        public bool IsAbsent { get; set; }
        public string? MarksRemarks { get; set; }
        public DateTime? MarksSubmittedAtUtc { get; set; }
        public int? MarksReviewedByUserId { get; set; }
        public DateTime? MarksReviewedAtUtc { get; set; }
        public string? MarksReviewComment { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public virtual Student Student { get; set; } = null!;
        public virtual ExamSession ExamSession { get; set; } = null!;
        public virtual ExamResultItem OriginalResultItem { get; set; } = null!;
        public virtual Teacher? Teacher { get; set; }
        public virtual User? ReviewedByUser { get; set; }
        public virtual User? MarksReviewedByUser { get; set; }
    }
}

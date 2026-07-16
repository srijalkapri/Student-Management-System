namespace CRUD.Domain.Models
{
    public class ExamResultItem
    {
        public int Id { get; set; }
        public int ExamResultBatchId { get; set; }
        public int StudentId { get; set; }
        public decimal? MarksObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public bool IsAbsent { get; set; }
        public string? Remarks { get; set; }

        public virtual ExamResultBatch ExamResultBatch { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}

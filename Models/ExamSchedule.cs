namespace CRUD.Models
{
    public enum ExamScheduleStatus
    {
        Draft,
        Published
    }

    public class ExamSchedule
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? AcademicYear { get; set; }
        public ExamScheduleStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Grade Grade { get; set; } = null!;
        public virtual ICollection<ExamSession> ExamSessions { get; set; } = new List<ExamSession>();
    }
}

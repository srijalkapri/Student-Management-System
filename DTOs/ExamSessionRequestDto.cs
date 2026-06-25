namespace CRUD.DTOs
{
    public class ExamSessionRequestDto
    {
        public DateTime? ExamDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? InvigilatorTeacherId { get; set; }
        public string? Notes { get; set; }
    }

    public class BulkUpdateSessionsRequestDto
    {
        public int ExamScheduleId { get; set; }
        public List<BulkUpdateSessionDto> Sessions { get; set; } = new List<BulkUpdateSessionDto>();
    }

    public class BulkUpdateSessionDto
    {
        public int Id { get; set; }
        public DateTime? ExamDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? InvigilatorTeacherId { get; set; }
        public string? Notes { get; set; }
    }

    public class AddExamSessionRequestDto
    {
        public int ExamScheduleId { get; set; }
        public int GradeSubjectId { get; set; }
        public DateTime? ExamDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? InvigilatorTeacherId { get; set; }
        public string? Notes { get; set; }
    }
}

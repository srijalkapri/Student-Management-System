

namespace CRUD.Domain.Models
{
    public class ExamSession
    {
        public int Id { get; set; }
        public int ExamScheduleId { get; set; }
        public int GradeSubjectId { get; set; }
        public DateTime? ExamDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? InvigilatorTeacherId { get; set; }
        public string? Notes { get; set; }

        public virtual ExamSchedule ExamSchedule { get; set; } = null!;
        public virtual GradeSubject GradeSubject { get; set; } = null!;
        public virtual Teacher? InvigilatorTeacher { get; set; }
    }
}

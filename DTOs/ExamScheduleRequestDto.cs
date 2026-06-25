using CRUD.Models;

namespace CRUD.DTOs
{
    public class ExamScheduleRequestDto
    {
        public int GradeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? AcademicYear { get; set; }
        public bool AutoGenerateSessions { get; set; }
    }

    public class UpdateExamScheduleRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string? AcademicYear { get; set; }
        public ExamScheduleStatus Status { get; set; }
    }
}

namespace CRUD.Application.DTOs
{
    public class ReExamRequestDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int ExamSessionId { get; set; }
        public int ExamScheduleId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int AttemptNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? StudentReason { get; set; }
        public string? AdminComment { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }

        public decimal? OriginalMarksObtained { get; set; }
        public decimal OriginalTotalMarks { get; set; }
        public bool OriginalIsAbsent { get; set; }

        public int? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public decimal? MarksObtained { get; set; }
        public decimal? TotalMarks { get; set; }
        public bool IsAbsent { get; set; }
        public string? MarksRemarks { get; set; }
        public DateTime? MarksSubmittedAtUtc { get; set; }
        public string? MarksReviewComment { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}

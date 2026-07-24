namespace CRUD.Application.DTOs
{
    public class StudentExamResultSubjectDto
    {
        public int ExamSessionId { get; set; }
        public int ExamResultItemId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public decimal? MarksObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public bool IsAbsent { get; set; }
        public string? Remarks { get; set; }

        public bool CanApplyReExam { get; set; }
        public string? ReExamStatus { get; set; }
        public int? ReExamRequestId { get; set; }
        public bool IsReExamResult { get; set; }
        public decimal? OriginalMarksObtained { get; set; }
        public decimal? OriginalTotalMarks { get; set; }
        public bool? OriginalIsAbsent { get; set; }
    }
}

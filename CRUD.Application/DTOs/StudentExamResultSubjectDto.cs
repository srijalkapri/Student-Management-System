namespace CRUD.Application.DTOs
{
    public class StudentExamResultSubjectDto
    {
        public string SubjectName { get; set; } = string.Empty;
        public decimal? MarksObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public bool IsAbsent { get; set; }
        public string? Remarks { get; set; }
    }
}

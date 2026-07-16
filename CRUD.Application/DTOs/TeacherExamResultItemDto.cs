namespace CRUD.Application.DTOs
{
    public class TeacherExamResultItemDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public decimal? MarksObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public bool IsAbsent { get; set; }
        public string? Remarks { get; set; }
    }
}

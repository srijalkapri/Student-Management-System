namespace CRUD.Application.DTOs
{
    public class TeacherSubmitReExamMarksDto
    {
        public decimal? MarksObtained { get; set; }
        public decimal TotalMarks { get; set; } = 100;
        public bool IsAbsent { get; set; }
        public string? Remarks { get; set; }
    }
}

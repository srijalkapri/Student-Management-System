namespace CRUD.Application.DTOs
{
    public class TeacherExamResultItemRequestDto
    {
        public int StudentId { get; set; }
        public decimal? MarksObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public bool IsAbsent { get; set; }
        public string? Remarks { get; set; }
    }
}

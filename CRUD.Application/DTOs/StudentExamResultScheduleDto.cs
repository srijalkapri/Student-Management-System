namespace CRUD.Application.DTOs
{
    public class StudentExamResultScheduleDto
    {
        public int ExamScheduleId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;
        public string? AcademicYear { get; set; }
        public List<StudentExamResultSubjectDto> Subjects { get; set; } = new();
        public decimal TotalObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal Percentage { get; set; }
    }
}

namespace CRUD.Application.DTOs
{
    public class AdminScheduleMarksDto
    {
        public int ExamScheduleId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;
        public string? AcademicYear { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public List<AdminStudentMarksRowDto> Students { get; set; } = new();
    }

    public class AdminStudentMarksRowDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public List<StudentExamResultSubjectDto> Subjects { get; set; } = new();
        public decimal TotalObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal Percentage { get; set; }
    }
}

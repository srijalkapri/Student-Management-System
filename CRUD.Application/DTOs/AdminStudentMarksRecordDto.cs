namespace CRUD.Application.DTOs
{
    public class AdminStudentMarksRecordDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
        public List<StudentExamResultScheduleDto> Results { get; set; } = new();
    }
}

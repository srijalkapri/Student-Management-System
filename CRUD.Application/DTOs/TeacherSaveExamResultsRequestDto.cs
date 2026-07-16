namespace CRUD.Application.DTOs
{
    public class TeacherSaveExamResultsRequestDto
    {
        public int ExamSessionId { get; set; }
        public List<TeacherExamResultItemRequestDto> Items { get; set; } = new();
    }
}

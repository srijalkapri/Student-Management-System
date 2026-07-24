namespace CRUD.Application.DTOs
{
    public class ApplyReExamRequestDto
    {
        public int ExamSessionId { get; set; }
        public string? Reason { get; set; }
    }
}

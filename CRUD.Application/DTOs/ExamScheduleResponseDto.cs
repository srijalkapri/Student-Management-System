using CRUD.Domain.Models;

namespace CRUD.Application.DTOs
{
    public class ExamScheduleResponseDto
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? AcademicYear { get; set; }
        public ExamScheduleStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ExamSessionResponseDto> Sessions { get; set; } = new List<ExamSessionResponseDto>();
    }
}

namespace CRUD.Application.DTOs
{
    public class PromoteStudentsResponseDto
    {
        public int PromotedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<SkippedStudentDto> SkippedStudents { get; set; } = new List<SkippedStudentDto>();
    }
}

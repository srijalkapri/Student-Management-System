namespace CRUD.DTOs
{
    public class TeacherDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GradeResponseDto> AssignedGrades { get; set; } = new List<GradeResponseDto>();
    }
}

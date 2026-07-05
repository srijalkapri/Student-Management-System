namespace CRUD.Application.DTOs
{
    public class GradeResponseDto
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int? ClassTeacherId { get; set; }
        public TeacherResponseDto? ClassTeacher { get; set; }
    }
}

namespace CRUD.DTOs
{
    public class GradeCreateDto
    {
        public string ClassName { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int TeacherId { get; set; }
    }
}

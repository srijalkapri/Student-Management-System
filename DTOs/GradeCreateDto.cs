namespace CRUD.DTOs
{
    public class GradeCreateDto
    {
        public string ClassName { get; set; } = string.Empty;
        public int? ClassTeacherId { get; set; }
    }
}

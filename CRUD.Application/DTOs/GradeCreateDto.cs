namespace CRUD.Application.DTOs
{
    public class GradeCreateDto
    {
        public string ClassName { get; set; } = string.Empty;
        public int Level { get; set; }
        public int? ClassTeacherId { get; set; }
    }
}

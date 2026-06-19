namespace CRUD.DTOs
{
    public class StudentCreate
    {
        public string Name { get; set; } = string.Empty;
        public string Grade { get; set; }= string.Empty;

        public int? TeacherId { get; set; }
    }
}

namespace CRUD.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string grade { get; set; } = string.Empty;
        public int TeacherId { get; set; }
    }
}

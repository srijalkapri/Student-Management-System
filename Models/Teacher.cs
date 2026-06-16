namespace CRUD.Models
{
    public class Teacher
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;


        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}

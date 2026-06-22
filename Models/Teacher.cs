namespace CRUD.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}   

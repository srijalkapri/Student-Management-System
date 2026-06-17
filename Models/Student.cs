namespace CRUD.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public int? TeacherId { get; set; }

        public virtual Teacher? Teacher { get; set; }
    }
}

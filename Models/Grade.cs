namespace CRUD.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}

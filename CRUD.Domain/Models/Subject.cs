namespace CRUD.Domain.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<GradeSubject> GradeSubjects { get; set; } = new List<GradeSubject>();
    }
}

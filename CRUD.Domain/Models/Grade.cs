

namespace CRUD.Domain.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int Level { get; set; }
        public int? ClassTeacherId { get; set; }

        public virtual Teacher? ClassTeacher { get; set; }
        public virtual ICollection<GradeSubject> GradeSubjects { get; set; } = new List<GradeSubject>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}

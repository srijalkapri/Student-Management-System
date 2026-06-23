namespace CRUD.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<GradeSubjectTeacher> GradeSubjectTeachers { get; set; } = new List<GradeSubjectTeacher>();
    }
}   

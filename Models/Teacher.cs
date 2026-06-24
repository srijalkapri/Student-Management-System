namespace CRUD.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;

        public virtual ICollection<GradeSubjectTeacher> GradeSubjectTeachers { get; set; } = new List<GradeSubjectTeacher>();
    }
}   

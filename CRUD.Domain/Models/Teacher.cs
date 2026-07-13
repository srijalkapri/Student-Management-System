namespace CRUD.Domain.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<GradeSubjectTeacher> GradeSubjectTeachers { get; set; } = new List<GradeSubjectTeacher>();
    }
}

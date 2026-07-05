

namespace CRUD.Domain.Models
{
    public class GradeSubject
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public bool IsOptional { get; set; }

        public virtual Grade Grade { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<GradeSubjectTeacher> GradeSubjectTeachers { get; set; } = new List<GradeSubjectTeacher>();
    }
}

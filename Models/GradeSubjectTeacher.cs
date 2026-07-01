namespace CRUD.Models
{
    public class GradeSubjectTeacher
    {
        public int Id { get; set; }
        public int GradeSubjectId { get; set; }
        public int TeacherId  { get; set; }

        public virtual GradeSubject GradeSubject { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
    }
}
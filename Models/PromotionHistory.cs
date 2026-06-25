namespace CRUD.Models
{
    public class PromotionHistory
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int FromGradeId { get; set; }
        public int ToGradeId { get; set; }
        public DateTime PromotedAt { get; set; }

        public virtual Student Student { get; set; } = null!;
        public virtual Grade FromGrade { get; set; } = null!;
        public virtual Grade ToGrade { get; set; } = null!;
    }
}

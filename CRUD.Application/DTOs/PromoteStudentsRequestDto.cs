namespace CRUD.Application.DTOs
{
    public class PromoteStudentsRequestDto
    {
        public int FromGradeId { get; set; }
        public int ToGradeId { get; set; }
        public List<int>? StudentIds { get; set; }
    }
}

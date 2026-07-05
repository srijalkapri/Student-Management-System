namespace CRUD.Application.DTOs
{
    public class GradeSubjectCreateDto
    {
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public bool IsOptional { get; set; }
    }
}

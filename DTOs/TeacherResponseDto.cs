namespace CRUD.DTOs
{
    public class TeacherResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public int TotalStudents { get; set; }


    }
}

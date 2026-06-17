namespace CRUD.DTOs
{
    public class TeacherResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<string> Grades { get; set; } = new List<string>();
        public int TotalStudents { get; set; }


    }
}

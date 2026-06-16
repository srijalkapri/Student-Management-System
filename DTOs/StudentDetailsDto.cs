namespace CRUD.DTOs
{
    public class StudentDetailsDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        public string StudentGrade { get; set; } = string.Empty;

        public string TeacherName { get; set; } = string.Empty;
        public string TeacherSubject { get; set; } = string.Empty;

    }
}

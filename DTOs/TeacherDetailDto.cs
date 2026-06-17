using CRUD.Models;


namespace CRUD.DTOs

{
    public class TeacherDetailDto
    {

        public string TeacherName { get; set; }= string.Empty;
        public List<string> StudentNames { get; set; } = new List<string>();
    }
}

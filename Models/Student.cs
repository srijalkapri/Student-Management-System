﻿namespace CRUD.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GradeId { get; set; }

        public virtual Grade Grade { get; set; } = null!;
    }
}

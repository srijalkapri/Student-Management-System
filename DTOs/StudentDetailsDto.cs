﻿namespace CRUD.DTOs
{
    public class StudentDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public List<GradeSubjectWithTeachersResponseDto> Subjects { get; set; } = new List<GradeSubjectWithTeachersResponseDto>();
    }
}

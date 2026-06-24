﻿﻿﻿﻿﻿﻿namespace CRUD.DTOs
{
    public class StudentDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int? ClassTeacherId { get; set; }
        public TeacherResponseDto? ClassTeacher { get; set; }
        public List<GradeSubjectWithTeachersResponseDto> Subjects { get; set; } = new List<GradeSubjectWithTeachersResponseDto>();
    }
}

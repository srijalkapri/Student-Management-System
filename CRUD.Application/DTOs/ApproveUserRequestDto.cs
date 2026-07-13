namespace CRUD.Application.DTOs
{
    public class ApproveUserRequestDto
    {
        public string Role { get; set; } = string.Empty;

        /// <summary>Link an existing teacher profile when approving as Teacher.</summary>
        public int? TeacherId { get; set; }

        /// <summary>Link an existing student profile when approving as Student.</summary>
        public int? StudentId { get; set; }

        /// <summary>Required when approving as Student without StudentId (creates a new student).</summary>
        public int? GradeId { get; set; }

        public string? PhoneNo { get; set; }
    }
}

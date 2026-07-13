using CRUD.Application.DTOs;
using FluentValidation;

namespace CRUD.Application.Validators
{
    public class ApproveUserRequestDtoValidator : AbstractValidator<ApproveUserRequestDto>
    {
        public ApproveUserRequestDtoValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(x => x is "SuperAdmin" or "Teacher" or "Student")
                .WithMessage("Role must be one of: SuperAdmin, Teacher, Student.");

            RuleFor(x => x.GradeId)
                .NotNull()
                .When(x => x.Role == "Student" && !x.StudentId.HasValue)
                .WithMessage("GradeId is required when creating a new student profile.");

            RuleFor(x => x.StudentId)
                .Null()
                .When(x => x.Role != "Student")
                .WithMessage("StudentId is only valid when Role is Student.");

            RuleFor(x => x.TeacherId)
                .Null()
                .When(x => x.Role != "Teacher")
                .WithMessage("TeacherId is only valid when Role is Teacher.");
        }
    }
}

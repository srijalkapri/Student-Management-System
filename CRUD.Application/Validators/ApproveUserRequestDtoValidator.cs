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
                .Must(x => x == "SuperAdmin" || x == "Teacher" || x == "Staff" || x == "User")  
                .WithMessage("Role must be one of: SuperAdmin, Teacher, Staff, User.");
        }
    }
}

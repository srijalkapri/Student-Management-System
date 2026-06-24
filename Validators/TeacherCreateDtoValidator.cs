using CRUD.DTOs;
using FluentValidation;

namespace CRUD.Validators
{
    public class TeacherCreateDtoValidator : AbstractValidator<TeacherCreateDto>
    {
        public TeacherCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name is too short.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(254).WithMessage("Email cannot exceed 254 characters.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Email format is invalid. (Example: user@domain.com)");

            RuleFor(x => x.PhoneNo)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.")
                .Matches(@"^[0-9+\-\(\)\s]+$").WithMessage("Phone number can only contain digits, spaces, hyphens, parentheses, and plus sign.");
        }
    }
}

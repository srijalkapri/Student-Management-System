using CRUD.Application.DTOs;
using FluentValidation;

namespace CRUD.Application.Validators
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username is too short (minimum 3 characters).")
                .MaximumLength(50).WithMessage("Username is too long (maximum 50 characters).")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password is too short (minimum 6 characters).")
                .MaximumLength(100).WithMessage("Password is too long (maximum 100 characters).");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.FullName)
                .MaximumLength(100).WithMessage("Full name is too long (maximum 100 characters).")
                .When(x => !string.IsNullOrEmpty(x.FullName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(254).WithMessage("Email is too long (maximum 254 characters).")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}

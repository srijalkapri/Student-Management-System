using CRUD.Application.DTOs;
using FluentValidation;

namespace CRUD.Application.Validators
{
    public class GradeCreateDtoValidator : AbstractValidator<GradeCreateDto>
    {
        public GradeCreateDtoValidator()
        {
            RuleFor(x => x.ClassName)
                .NotEmpty().WithMessage("Class name is required.")
                .MaximumLength(50).WithMessage("Class name is too long (maximum 50 characters).");

            RuleFor(x => x.Level)
                .GreaterThan(0).WithMessage("Grade level must be greater than zero.");
        }
    }
}

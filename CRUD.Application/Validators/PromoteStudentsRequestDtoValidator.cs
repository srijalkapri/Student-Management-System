using CRUD.Application.DTOs;
using FluentValidation;

namespace CRUD.Application.Validators
{
    public class PromoteStudentsRequestDtoValidator : AbstractValidator<PromoteStudentsRequestDto>
    {
        public PromoteStudentsRequestDtoValidator()
        {
            RuleFor(x => x.FromGradeId)
                .GreaterThan(0).WithMessage("Source grade is required.");

            RuleFor(x => x.ToGradeId)
                .GreaterThan(0).WithMessage("Target grade is required.");

            RuleFor(x => x.ToGradeId)
                .NotEqual(x => x.FromGradeId)
                .WithMessage("Source and target grades must be different.");
        }
    }
}

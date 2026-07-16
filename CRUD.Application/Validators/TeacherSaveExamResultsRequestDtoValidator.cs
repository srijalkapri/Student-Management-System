using CRUD.Application.DTOs;
using FluentValidation;

namespace CRUD.Application.Validators
{
    public class TeacherSaveExamResultsRequestDtoValidator : AbstractValidator<TeacherSaveExamResultsRequestDto>
    {
        public TeacherSaveExamResultsRequestDtoValidator()
        {
            RuleFor(x => x.ExamSessionId)
                .GreaterThan(0);

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one student result is required.");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.StudentId).GreaterThan(0);
                items.RuleFor(i => i.TotalMarks).GreaterThan(0);
                items.RuleFor(i => i.MarksObtained)
                    .GreaterThanOrEqualTo(0)
                    .When(i => i.MarksObtained.HasValue);
                items.RuleFor(i => i)
                    .Must(i => i.IsAbsent || (i.MarksObtained.HasValue && i.MarksObtained.Value <= i.TotalMarks))
                    .WithMessage("Marks obtained cannot exceed total marks for non-absent students.");
            });
        }
    }
}

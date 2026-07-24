using CRUD.Application.DTOs;
using FluentValidation;

namespace CRUD.Application.Validators
{
    public class ApplyReExamRequestDtoValidator : AbstractValidator<ApplyReExamRequestDto>
    {
        public ApplyReExamRequestDtoValidator()
        {
            RuleFor(x => x.ExamSessionId).GreaterThan(0);
            RuleFor(x => x.Reason).MaximumLength(500).When(x => x.Reason != null);
        }
    }

    public class TeacherSubmitReExamMarksDtoValidator : AbstractValidator<TeacherSubmitReExamMarksDto>
    {
        public TeacherSubmitReExamMarksDtoValidator()
        {
            RuleFor(x => x.TotalMarks).GreaterThan(0);
            RuleFor(x => x.Remarks).MaximumLength(300).When(x => x.Remarks != null);
            RuleFor(x => x.MarksObtained)
                .NotNull()
                .When(x => !x.IsAbsent)
                .WithMessage("Marks obtained is required when student is not absent.");
            RuleFor(x => x)
                .Must(x => x.IsAbsent || x.MarksObtained == null || (x.MarksObtained >= 0 && x.MarksObtained <= x.TotalMarks))
                .WithMessage("Marks obtained must be between 0 and total marks.");
        }
    }
}

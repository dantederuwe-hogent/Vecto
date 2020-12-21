using FluentValidation;
using Vecto.Application.Helpers;
using Vecto.Core.Entities;

namespace Vecto.Application.Sections
{
    public class SectionValidator : AbstractValidator<SectionDTO>
    {
        public SectionValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty()
                .MaximumLength(100);

            var validTypes = typeof(Section).GetSubtypeNamesInSameAssembly();
            string allowed = string.Join(", ", validTypes);

            RuleFor(s => s.SectionType)
                .NotEmpty()
                .Must(validTypes.Contains).WithMessage($"must be one of the following: {allowed}");
        }
    }
}

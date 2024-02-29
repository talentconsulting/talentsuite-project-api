using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentConsulting.TalentSuite.Projects.API.Commands.UpdateSow;

[ExcludeFromCodeCoverage]
public class UpdateSowCommandValidator : AbstractValidator<UpdateSowCommand>
{
    public UpdateSowCommandValidator()
    {
        
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.SowDto)
            .NotNull();
    }
}


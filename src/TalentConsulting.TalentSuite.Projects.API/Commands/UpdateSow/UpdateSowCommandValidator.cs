using FluentValidation;

namespace TalentConsulting.TalentSuite.Projects.API.Commands.UpdateSow;

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


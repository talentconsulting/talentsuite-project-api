using FluentValidation;

namespace TalentConsulting.TalentSuite.Projects.API.Commands.UpdateSow;

public class DeleteSowCommandValidator : AbstractValidator<UpdateSowCommand>
{
    public DeleteSowCommandValidator()
    {
        
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.SowDto)
            .NotNull();
    }
}


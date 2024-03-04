using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentConsulting.TalentSuite.Projects.API.Queries.GetSows;

[ExcludeFromCodeCoverage]
public class GetSowByProjectIdAndSowIdCommandValidator : AbstractValidator<GetSowByProjectIdAndSowIdCommand>
{
    public GetSowByProjectIdAndSowIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.ProjectId)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();
    }
}

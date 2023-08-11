using TalentConsulting.TalentSuite.Projects.Common.Entities;

namespace TalentConsulting.TalentSuite.Projects.Core.Interfaces.Commands;

public interface IUpdateProjectCommand
{
    string Id { get; }
    ProjectDto ProjectDto { get; }
}

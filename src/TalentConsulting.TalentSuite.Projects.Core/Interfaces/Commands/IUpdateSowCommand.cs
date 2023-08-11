using TalentConsulting.TalentSuite.Projects.Common.Entities;

namespace TalentConsulting.TalentSuite.Projects.Core.Interfaces.Commands;

public interface IUpdateSowCommand
{
    string Id { get; }
    SowDto SowDto { get; }
}

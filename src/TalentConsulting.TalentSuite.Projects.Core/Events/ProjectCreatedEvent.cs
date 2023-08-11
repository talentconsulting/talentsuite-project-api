using TalentConsulting.TalentSuite.Projects.Common;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Core.Events;



public interface IProjectCreatedEvent
{ 
    Project Item { get; }
}

public class ProjectCreatedEvent : DomainEventBase, IProjectCreatedEvent
{
    public ProjectCreatedEvent(Project item)
    {
        Item = item;
    }

    public Project Item { get; }
}


using MediatR;

namespace TalentConsulting.TalentSuite.Projects.Common;

public abstract class DomainEventBase : INotification
{
    public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.UtcNow;
}


using MediatR;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Projects.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Projects.Common;

[ExcludeFromCodeCoverage]
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAndClearEvents(IEnumerable<EntityBase<string>> entitiesWithEvents)
    {
        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent).ConfigureAwait(false);
            }
        }
    }
}

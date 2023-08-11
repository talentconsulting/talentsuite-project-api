namespace TalentConsulting.TalentSuite.Projects.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<EntityBase<string>> entitiesWithEvents);
}

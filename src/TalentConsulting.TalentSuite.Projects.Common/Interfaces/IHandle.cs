namespace TalentConsulting.TalentSuite.Projects.Common.Interfaces;

public interface IHandle<T> where T : DomainEventBase
{
    Task HandleAsync(T args);
}

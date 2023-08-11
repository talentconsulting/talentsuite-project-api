namespace TalentConsulting.TalentSuite.Projects.Common.Interfaces;

public interface IHandle<in T> where T : DomainEventBase
{
    Task HandleAsync(T args);
}

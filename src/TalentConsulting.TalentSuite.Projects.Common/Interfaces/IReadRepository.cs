using Ardalis.Specification;

namespace TalentConsulting.TalentSuite.Projects.Common.Interfaces;


public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}

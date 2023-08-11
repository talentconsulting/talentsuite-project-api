﻿using Ardalis.Specification;

namespace TalentConsulting.TalentSuite.Projects.Common.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}


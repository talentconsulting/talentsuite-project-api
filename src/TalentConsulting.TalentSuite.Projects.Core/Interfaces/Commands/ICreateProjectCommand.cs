﻿using TalentConsulting.TalentSuite.Projects.Common.Entities;

namespace TalentConsulting.TalentSuite.Projects.Core.Interfaces.Commands;

public interface ICreateProjectCommand
{
    ProjectDto ProjectDto { get; }
}

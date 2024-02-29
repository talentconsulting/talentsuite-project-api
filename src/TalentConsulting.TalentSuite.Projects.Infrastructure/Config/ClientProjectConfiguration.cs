﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Config;

[ExcludeFromCodeCoverage]
public class ClientProjectConfiguration
{
    public void Configure(EntityTypeBuilder<ClientProject> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.ClientId)
            .IsRequired();
        builder.Property(t => t.ProjectId)
            .IsRequired();

    }
}

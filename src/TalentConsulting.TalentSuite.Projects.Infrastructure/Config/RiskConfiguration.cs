﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Config;

public class RiskConfiguration
{
    public void Configure(EntityTypeBuilder<Risk> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.ReportId)
            .IsRequired();
        builder.Property(t => t.RiskDetails)
            .IsRequired();
        builder.Property(t => t.RiskMitigation)
            .IsRequired();
        builder.Property(t => t.RagStatus)
            .IsRequired();

        builder.Property(t => t.Created)
            .IsRequired();
    }
}

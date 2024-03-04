using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Config;

[ExcludeFromCodeCoverage]
public class SowConfiguration
{
    public void Configure(EntityTypeBuilder<Sow> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.IsChangeRequest)
            .IsRequired();
        builder.Property(t => t.SowStartDate)
            .IsRequired();
        builder.Property(t => t.SowEndDate)
            .IsRequired();
        builder.Property(t => t.ProjectId)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
    }
}

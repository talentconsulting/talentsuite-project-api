﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Config;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.ContactNumber)
            .IsRequired();
        builder.Property(t => t.Name)
            .IsRequired();
        builder.Property(t => t.Reference)
            .IsRequired();
        builder.Property(t => t.StartDate)
            .IsRequired();
        builder.Property(t => t.EndDate)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();

        builder.HasMany(s => s.ClientProjects)
            .WithOne()
            .HasForeignKey(lc => lc.ProjectId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Contacts)
            .WithOne()
            .HasForeignKey(lc => lc.ProjectId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Reports)
            .WithOne()
            .HasForeignKey(lc => lc.ProjectId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Sows)
            .WithOne()
            .HasForeignKey(lc => lc.ProjectId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

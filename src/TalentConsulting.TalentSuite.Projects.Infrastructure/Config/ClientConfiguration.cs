﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Config;

public class ClientConfiguration
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .IsRequired();
        builder.Property(t => t.ContactName)
            .IsRequired();
        builder.Property(t => t.ContactEmail)
            .IsRequired();

        builder.Property(t => t.Created)
            .IsRequired();


        builder.HasMany(s => s.ClientProjects)
            .WithOne()
            .HasForeignKey(lc => lc.ClientId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

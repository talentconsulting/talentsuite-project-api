using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Core.Infrastructure;

public interface IApplicationDbContext
{
    public DbSet<Report> Reports { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

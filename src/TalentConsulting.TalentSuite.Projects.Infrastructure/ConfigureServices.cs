using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentConsulting.TalentSuite.Projects.Common;
using TalentConsulting.TalentSuite.Projects.Common.Interfaces;
using TalentConsulting.TalentSuite.Projects.Core.Infrastructure;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Interceptors;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Repository;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Service;

namespace TalentConsulting.TalentSuite.Projects.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        var useDbType = configuration.GetValue<string>("UseDbType");

        switch (useDbType)
        {
            case "UseInMemoryDatabase":
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TalentDb"));
                break;

            case "UseSqlServerDatabase":
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? String.Empty));
                break;

            case "UseSqlLite":
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection") ?? String.Empty));
                break;

            case "UsePostgresDatabase":
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ?? String.Empty));
                break;

            default:
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TalentDb"));
                break;
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}

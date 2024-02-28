using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Repository;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync(bool isProduction, bool restartDatabase)
    {
        try
        {
            if (_context.Database.IsInMemory())
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();
            }

            if (_context.Database.IsSqlite())
            {
                if (restartDatabase)
                {
                    await _context.Database.EnsureDeletedAsync();
                }
                await _context.Database.EnsureCreatedAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (_context.Projects.Any())
            return;

        if (_context.Database.IsInMemory() || _context.Database.IsSqlite())
        {
            _context.Projects.Add(new Core.Entities.Project(new Guid("86b610ee-e866-4749-9f10-4a5c59e96f2f"), "0121 111 2222", "Social work CPD", "con_23sds", new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
                new List<ClientProject>(),
                new List<Contact>(),
                new List<Report>(),
                new List<Sow>()));

            _context.Contacts.Add(new Contact(new Guid("03a33a03-a98d-4946-8e8f-05cbc7a949b6"), "Ron Weasley", "ron@weasley.com", true, new Guid("86b610ee-e866-4749-9f10-4a5c59e96f2f")));

            _context.Clients.AddRange(Clients().ToArray());

            _context.ProjectRoles.AddRange(ProjectRoles().ToArray());

            await _context.SaveChangesAsync();
        }  
    }

    private static List<Client> Clients() 
    {
        return new List<Client>
        {
            new Core.Entities.Client(new Guid("83c756a8-ff87-48be-a862-096678b41817"), "Harry Potter", "DfE", "harry@potter.com", new List<ClientProject>(){ new ClientProject(new Guid("83c756a8-ff87-48be-a862-096678b41817"), new Guid("519df403-0e0d-4c25-b240-8d9ca21132b8"), new Guid("86b610ee-e866-4749-9f10-4a5c59e96f2f")) } ),
            new Core.Entities.Client(new Guid("e24a5543-6368-490a-a1d0-a18f0c69848a"), "Hermione Granger", "ESFA", "hermione@granger.com", new List<ClientProject>(){ new ClientProject(new Guid("51104a18-0e62-415b-91bc-6a0b83abceca"), new Guid("e24a5543-6368-490a-a1d0-a18f0c69848a"), new Guid("86b610ee-e866-4749-9f10-4a5c59e96f2f")) } )
        };
    }

    private static List<ProjectRole> ProjectRoles()
    {
        return new List<ProjectRole>
        {
            new ProjectRole(new Guid("626bff24-61c7-49d3-81bd-4aa12311e103"), "Developer", true, "Developer on the project writing the code" ),
            new ProjectRole(new Guid("fe32f237-ce7e-48f2-add7-fa5dc725396c"), "Architect", true, "Over seeing the architecture of the project" ),
            new ProjectRole(new Guid("4d3e2d9e-53fe-4a98-9062-9353a54bdece"), "Business Analyst", false, "Analysing business needs" ),
            new ProjectRole(new Guid("ed939f0b-4793-4e7c-82fa-f621cb0d8785"), "Architect", false, "Over seeing the architecture of the project" )
        };
    }
}

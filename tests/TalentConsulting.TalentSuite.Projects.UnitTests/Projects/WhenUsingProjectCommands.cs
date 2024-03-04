using Ardalis.GuardClauses;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TalentConsulting.TalentSuite.Projects.API.Commands.CreateProject;
using TalentConsulting.TalentSuite.Projects.API.Commands.UpdateProject;
using TalentConsulting.TalentSuite.Projects.API.Queries.GetProjects;
using TalentConsulting.TalentSuite.Projects.Common.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.UnitTests.Projects;

public class WhenUsingProjectCommands : BaseCreateDbUnitTest
{

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ThenCreateProject(bool newProjectId)
    {
        //Arrange
        var logger = new Mock<ILogger<CreateProjectCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        if (newProjectId)
        {
            var project = GetTestProject();
            mockApplicationDbContext.Projects.Add(project);
            mockApplicationDbContext.SaveChanges();
        }
        var testProject = GetTestProjectDto(newProjectId);
        var command = new CreateProjectCommand(testProject);
        var handler = new CreateProjectCommandHandler(mockApplicationDbContext, _mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testProject.Id);
    }



    [Fact]
    public async Task ThenHandle_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Arrange
        var logger = new Logger<CreateProjectCommandHandler>(new LoggerFactory());
        var handler = new CreateProjectCommandHandler(GetApplicationDbContext(), _mapper, logger);
        var command = new CreateProjectCommand(default!);

        // Act
        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task ThenUpdateProject()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbProject = GetTestProject();
        mockApplicationDbContext.Projects.Add(dbProject);
        await mockApplicationDbContext.SaveChangesAsync();
        var testProject = GetTestProjectDto();
        var logger = new Mock<ILogger<UpdateProjectCommandHandler>>();

        var command = new UpdateProjectCommand("a3226044-5c89-4257-8b07-f29745a22e2c", testProject);
        var handler = new UpdateProjectCommandHandler(mockApplicationDbContext, _mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testProject.Id);
    }

    [Fact]
    public async Task ThenUpdateProjectUsingTheSameProject()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbProject = GetTestProject();
        mockApplicationDbContext.Projects.Add(dbProject);
        await mockApplicationDbContext.SaveChangesAsync();
        var testProject = GetTestProjectDto();
        var logger = new Mock<ILogger<UpdateProjectCommandHandler>>();

        var command = new UpdateProjectCommand("a3226044-5c89-4257-8b07-f29745a22e2c", testProject);
        var handler = new UpdateProjectCommandHandler(mockApplicationDbContext, _mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testProject.Id);

        //Act
        var result2 = await handler.Handle(command, new CancellationToken());

        //Assert
        result2.Should().NotBeNull();
        result2.Should().Be(testProject.Id);
    }

    [Fact]
    public async Task ThenHandle_ThrowsException_WhenProjectNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var dbProject = GetTestProject();
        dbContext.Projects.Add(dbProject);
        await dbContext.SaveChangesAsync();
        var logger = new Mock<ILogger<UpdateProjectCommandHandler>>();
        var handler = new UpdateProjectCommandHandler(dbContext, _mapper, logger.Object);
        var command = new UpdateProjectCommand("a3226044-5c89-4257-8b07-f29745a22e2c", default!);

        // Act
        //Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task ThenHandle_ThrowsNotFoundException_WhenProjectIdNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var logger = new Mock<ILogger<UpdateProjectCommandHandler>>();
        var handler = new UpdateProjectCommandHandler(dbContext, _mapper, logger.Object);
        var command = new UpdateProjectCommand("a3226044-5c89-4257-8b07-f29745a22e2c", default!);

        // Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task ThenGetProject()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbProject = GetTestProject();
        mockApplicationDbContext.Projects.Add(dbProject);
        await mockApplicationDbContext.SaveChangesAsync();


        var command = new GetProjectsCommand(1, 99);
        var handler = new GetProjectsCommandHandler(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Id.Should().Be("a3226044-5c89-4257-8b07-f29745a22e2c");
        result.Items[0].Name.Should().Be(dbProject.Name);

    }

    [Fact]
    public async Task ThenGetProjectWithNullRequest()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbProject = GetTestProject();
        mockApplicationDbContext.Projects.Add(dbProject);
        await mockApplicationDbContext.SaveChangesAsync();
        var handler = new GetProjectsCommandHandler(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(new GetProjectsCommand(1, 99), new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Id.Should().Be("a3226044-5c89-4257-8b07-f29745a22e2c");
        result.Items[0].Name.Should().Be(dbProject.Name);
    }

    [Fact]
    public async Task ThenGetProjectById()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbProject = GetTestProject();
        mockApplicationDbContext.Projects.Add(dbProject);
        await mockApplicationDbContext.SaveChangesAsync();


        var command = new GetProjectByIdCommand(dbProject.Id.ToString());
        var handler = new GetProjectByIdCommandHandler(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("a3226044-5c89-4257-8b07-f29745a22e2c");
        result.Name.Should().Be(dbProject.Name);

    }

    [Fact]
    public async Task ThenGetProjectById_ThatDoesNotExist()
    {
        var mockApplicationDbContext = GetApplicationDbContext();

        var command = new GetProjectByIdCommand("8f145d0c-2b07-4beb-8a7f-d66055b88dc0");
        var handler = new GetProjectByIdCommandHandler(mockApplicationDbContext);

        // Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

    }

    public static Project GetTestProject()
    {
        return new Project(_projectId, "0121 111 2222", "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProject>()
            {
                new ClientProject(new Guid("d0ec8781-28ed-43dc-8840-e18ac1d255e8"),_clientId,_projectId)
            },
            new List<Contact>()
            {
                new Contact(new Guid("8585578d-8ac0-4613-96ff-89403c56a2c7"), "firstname", "email@email.com", true, _projectId)
            },
            new List<Report>()
            {
                new Report(_reportId, "Planned tasks", "Completed tasks", 1, DateTime.UtcNow, _projectId, _userId,
                new List<Risk>()
                {
                    new Risk(_riskId, _reportId, "Risk Details", "Risk Mitigation", "Risk Status" )
                }
                )
            },
            new List<Core.Entities.Sow>()
            {
                new Core.Entities.Sow(new Guid("946c4c15-913c-42e1-947d-b813b90f4d81"), DateTime.UtcNow, new List<SowFile>
                {
                    new SowFile
                    {
                        Id = new Guid("bce1bb9c-36aa-4f63-9cba-d8b435a79637"),
                        Mimetype = "application/pdf",
                        Filename = "document.pdf",
                        Size = 1024,
                        SowId = new Guid("946c4c15-913c-42e1-947d-b813b90f4d81"),
                        File = new byte[] { 0x12, 0x34, 0x56, 0x78 }
                    }
                }
                , true, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), _projectId)
            });
    }

    public static ProjectDto GetTestProjectDto(bool changeProjectId = false)
    {
        string projectId = _projectId.ToString();
        
        if (changeProjectId)
        {
            projectId = Guid.NewGuid().ToString();
        }

        var risks = new List<RiskDto>()
        {
            new RiskDto(_riskId.ToString(), _reportId.ToString(), "Risk Details", "Risk Mitigation", "Risk Status" )
        };

        return new ProjectDto(projectId, "0121 111 2222", "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>()
            {
                new ClientProjectDto("d0ec8781-28ed-43dc-8840-e18ac1d255e8",_clientId.ToString(),projectId)
            },
            new List<ContactDto>()
            {
                new ContactDto("8585578d-8ac0-4613-96ff-89403c56a2c7", "firstname", "email@email.com", true, projectId)
            },
            new List<ReportDto>()
            {
                new ReportDto(_reportId.ToString(), DateTime.UtcNow.AddDays(-1), "Planned tasks", "Completed tasks", 1, DateTime.UtcNow, projectId, _userId.ToString(),risks)
            },
            new List<SowDto>()
            {
                new SowDto("946c4c15-913c-42e1-947d-b813b90f4d81", DateTime.UtcNow, new List<SowFileDto>
                {
                    new SowFileDto
                    {
                        Id = "bce1bb9c-36aa-4f63-9cba-d8b435a79637",
                        Mimetype = "application/pdf",
                        Filename = "document.pdf",
                        Size = 1024,
                        SowId = "946c4c15-913c-42e1-947d-b813b90f4d81",
                        File = new byte[] { 0x12, 0x34, 0x56, 0x78 }
                    }

                }, true, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), projectId)
            });
    }
}

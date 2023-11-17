using Ardalis.GuardClauses;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TalentConsulting.TalentSuite.Projects.API.Commands.DeleteSow;
using TalentConsulting.TalentSuite.Projects.API.Commands.UpdateSow;
using TalentConsulting.TalentSuite.Projects.API.Queries.GetSows;
using TalentConsulting.TalentSuite.Projects.UnitTests.Projects;

namespace TalentConsulting.TalentSuite.Projects.UnitTests.Sow;

public class WhenUsingSowCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenGetSowByProjectIdAndSowId()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbProject = WhenUsingProjectCommands.GetTestProject();
        mockApplicationDbContext.Projects.Add(dbProject);
        await mockApplicationDbContext.SaveChangesAsync();


        var command = new GetSowByProjectIdAndSowIdCommand("a3226044-5c89-4257-8b07-f29745a22e2c", "946c4c15-913c-42e1-947d-b813b90f4d81");
        var handler = new GetSowByProjectIdAndSowIdCommandHandler(mockApplicationDbContext, _mapper);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("946c4c15-913c-42e1-947d-b813b90f4d81");
        result.ProjectId.Should().Be("a3226044-5c89-4257-8b07-f29745a22e2c");
    }

    [Fact]
    public async Task ThenHandle_ThrowsNotFoundException_WhenProjectIdNotFound()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var command = new GetSowByProjectIdAndSowIdCommand("a3226044-5c89-4257-8b07-f29745a22e2c", "946c4c15-913c-42e1-947d-b813b90f4d81");
        var handler = new GetSowByProjectIdAndSowIdCommandHandler(mockApplicationDbContext, _mapper);

        // Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task ThenDeleteSowByProjectIdAndSowId()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbProject = WhenUsingProjectCommands.GetTestProject();
        mockApplicationDbContext.Projects.Add(dbProject);
        await mockApplicationDbContext.SaveChangesAsync();


        var command = new DeleteSowCommand("a3226044-5c89-4257-8b07-f29745a22e2c", "946c4c15-913c-42e1-947d-b813b90f4d81");
        var handler = new DeleteSowCommandHandler(mockApplicationDbContext, new Mock<ILogger<DeleteSowCommandHandler>>().Object);

        //Act
        bool result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ThenUpdateSow()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbProject = WhenUsingProjectCommands.GetTestProject();
        mockApplicationDbContext.Projects.Add(dbProject);
        await mockApplicationDbContext.SaveChangesAsync();
        var sow = WhenUsingProjectCommands.GetTestProjectDto().Sows.ElementAt(0);

        var command = new UpdateSowCommand(sow.Id, sow);
        var handler = new UpdateSowCommandHandler(mockApplicationDbContext, _mapper, new Mock<ILogger<UpdateSowCommandHandler>>().Object);

        //Act
        string result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().Be(sow.Id);
    }
}

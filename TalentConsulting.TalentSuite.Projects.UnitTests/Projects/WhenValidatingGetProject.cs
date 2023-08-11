using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentConsulting.TalentSuite.Projects.API.Queries.GetProjects;

namespace TalentConsulting.TalentSuite.Projects.UnitTests.Projects;

public class WhenValidatingGetProject
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValidWhenGettingProjectById()
    {
        //Arrange
        var validator = new GetProjectByIdCommandValidator();
        var testModel = new GetProjectByIdCommand("a3226044-5c89-4257-8b07-f29745a22e2c");


        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData(default!)]
    [InlineData("")]
    public void ThenShouldErrorWhenModelIsNoptValidWhenGettingProjectById(string id)
    {
        //Arrange
        var validator = new GetProjectByIdCommandValidator();
        var testModel = new GetProjectByIdCommand(id);


        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }
}

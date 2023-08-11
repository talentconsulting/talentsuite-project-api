using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentConsulting.TalentSuite.Projects.API.Commands.UpdateProject;
using TalentConsulting.TalentSuite.Projects.Common.Entities;

namespace TalentConsulting.TalentSuite.Projects.UnitTests.Projects;

public class WhenValidatingUpdateProject : BaseTestValidation
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new UpdateProjectCommandValidator();
        var testModel = new UpdateProjectCommand("Id", new ProjectDto(_projectId, "0121 111 2222", "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>(),
            new List<ContactDto>(),
            new List<ReportDto>(),
            new List<SowDto>()));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenHasNoId()
    {
        //Arrange
        var validator = new UpdateProjectCommandValidator();
        var testModel = new UpdateProjectCommand("", new ProjectDto("", "0121 111 2222", "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>(),
            new List<ContactDto>(),
            new List<ReportDto>(),
            new List<SowDto>()));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Exists(x => x.PropertyName == "ProjectDto.Id").Should().BeTrue();
    }

    [Theory]
    [InlineData(default!)]
    [InlineData("")]
    public void ThenShouldErrorWhenModelHasNoId(string id)
    {
        //Arrange
        var validator = new UpdateProjectCommandValidator();
        var testModel = new UpdateProjectCommand(id, new ProjectDto(id, "0121 111 2222", "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>(),
            new List<ContactDto>(),
            new List<ReportDto>(),
            new List<SowDto>()));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Exists(x => x.PropertyName == "ProjectDto.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var validator = new UpdateProjectCommandValidator();
        var testModel = new UpdateProjectCommand("Id", new ProjectDto(_projectId, "0121 111 2222", default!, "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>(),
            new List<ContactDto>(),
            new List<ReportDto>(),
            new List<SowDto>()));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Exists(x => x.PropertyName == "ProjectDto.Name").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoContactNumber()
    {
        //Arrange
        var validator = new UpdateProjectCommandValidator();
        var testModel = new UpdateProjectCommand(_projectId, new ProjectDto(_projectId, default!, "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>(),
            new List<ContactDto>(),
            new List<ReportDto>(),
            new List<SowDto>()));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Exists(x => x.PropertyName == "ProjectDto.ContactNumber").Should().BeTrue();
    }
    [Fact]
    public void ThenShouldErrorWhenModelHasNoReferance()
    {
        //Arrange
        var validator = new UpdateProjectCommandValidator();
        var testModel = new UpdateProjectCommand(_projectId, new ProjectDto(_projectId, "0121 111 2222", "Social work CPD", default!, new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>(),
            new List<ContactDto>(),
            new List<ReportDto>(),
            new List<SowDto>()));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Exists(x => x.PropertyName == "ProjectDto.Reference").Should().BeTrue();
    }


}
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentConsulting.TalentSuite.Projects.API.Commands.CreateProject;
using TalentConsulting.TalentSuite.Projects.Common.Entities;

namespace TalentConsulting.TalentSuite.Projects.UnitTests.Projects;

public class WhenValidatingCreateProject : BaseTestValidation
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new CreateProjectCommandValidator();
        var testModel = new CreateProjectCommand(new ProjectDto(_projectId, "0121 111 2222", "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>(),
            new List<ContactDto>(),
            new List<ReportDto>(),
            new List<SowDto>()));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData(default!)]
    [InlineData("")]
    public void ThenShouldErrorWhenModelHasNoId(string id)
    {
        //Arrange
        var validator = new CreateProjectCommandValidator();
        var testModel = new CreateProjectCommand(new ProjectDto(id, "0121 111 2222", "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
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
        var validator = new CreateProjectCommandValidator();
        var testModel = new CreateProjectCommand(new ProjectDto(_projectId, "0121 111 2222", default!, "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
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
        var validator = new CreateProjectCommandValidator();
        var testModel = new CreateProjectCommand(new ProjectDto(_projectId, default!, "Social work CPD", "con_23sds", new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
            new List<ClientProjectDto>(),
            new List<ContactDto>(),
            new List<ReportDto>(),
            new List<SowDto>()));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Exists(x => x.PropertyName == "ProjectDto.ContractNumber").Should().BeTrue();
    }
    [Fact]
    public void ThenShouldErrorWhenModelHasNoReferance()
    {
        //Arrange
        var validator = new CreateProjectCommandValidator();
        var testModel = new CreateProjectCommand(new ProjectDto(_projectId, "0121 111 2222", "Social work CPD", default!, new DateTime(2023, 10, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 03, 31, 0, 0, 0, DateTimeKind.Utc),
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
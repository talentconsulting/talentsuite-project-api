using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TalentConsulting.TalentSuite.Projects.API.Commands.CreateProject;
using TalentConsulting.TalentSuite.Projects.API.Commands.UpdateProject;
using TalentConsulting.TalentSuite.Projects.API.Queries.GetProjects;
using TalentConsulting.TalentSuite.Projects.Common.Entities;

namespace TalentConsulting.TalentSuite.Projects.API.Endpoints;

public class MinimalProjectEndPoints
{
    public void RegisterProjectEndPoints(WebApplication app)
    {
        app.MapPost("api/projects", [Authorize(Policy = "TalentConsultingUser")] async ([FromBody] ProjectDto request, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                CreateProjectCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Projects", "Create project") { Tags = new[] { "Projects" } });

        app.MapPut("api/projects/{id}", [Authorize(Policy = "TalentConsultingUser")] async (string id, [FromBody] ProjectDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalProjectEndPoints> logger) =>
        {
            try
            {
                UpdateProjectCommand command = new(id, request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating project (api). {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Project", "Update Project By Id") { Tags = new[] { "Projects" } });



        app.MapGet("api/project/{id}", [Authorize(Policy = "TalentConsultingUser")] async (string id, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                GetProjectByIdCommand request = new(id);
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Project", "Get Project By Id") { Tags = new[] { "Projects" } });

        app.MapGet("api/projects", [Authorize(Policy = "TalentConsultingUser")] async (int? pageNumber, int? pageSize, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                GetProjectsCommand request = new(pageNumber, pageSize);
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Projects", "Get Projects Paginated") { Tags = new[] { "Projects" } });


    }
}

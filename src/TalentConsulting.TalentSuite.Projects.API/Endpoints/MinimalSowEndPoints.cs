using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TalentConsulting.TalentSuite.Projects.API.Commands.DeleteSow;
using TalentConsulting.TalentSuite.Projects.API.Commands.UpdateSow;
using TalentConsulting.TalentSuite.Projects.API.Queries.GetSows;
using TalentConsulting.TalentSuite.Projects.Common.Entities;

namespace TalentConsulting.TalentSuite.Projects.API.Endpoints;

public class MinimalSowEndPoints
{
    public void RegisterSowEndPoints(WebApplication app)
    {
        app.MapGet("projects/{projectId}/sows/{sowId}", [Authorize(Policy = "TalentConsultingUser")] async (string projectId, string id, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                GetSowByProjectIdAndSowIdCommand request = new(projectId, id);
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Sow", "Get Sow By Prject Id and Id") { Tags = new[] { "Sows" } });

        app.MapPut("api/sows", [Authorize(Policy = "TalentConsultingUser")] async (string id, [FromBody] SowDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalProjectEndPoints> logger) =>
        {
            try
            {
                UpdateSowCommand command = new(id, request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating sow (api). {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Sow", "Update Sow By Id") { Tags = new[] { "Sows" } });

        app.MapDelete("projects/{projectId}/sows/{sowId}", [Authorize(Policy = "TalentConsultingUser")] async (string projectId, string id, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalProjectEndPoints> logger) =>
        {
            try
            {
                DeleteSowCommand command = new(projectId, id);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred deleting sow (api). {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Delete Sow", "Delete Sow By Id") { Tags = new[] { "Sows" } });
    }
}

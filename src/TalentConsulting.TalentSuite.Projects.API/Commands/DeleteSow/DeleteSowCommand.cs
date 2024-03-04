using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Projects.Core.Entities;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Projects.API.Commands.DeleteSow;

public class DeleteSowCommand : IRequest<bool>
{
    public DeleteSowCommand(string projectId, string id)
    {
        Id = id;
        ProjectId = projectId;
    }

    public string Id { get; }
    public string ProjectId { get; }

}

public class DeleteSowCommandHandler : IRequestHandler<DeleteSowCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DeleteSowCommandHandler> _logger;

    public DeleteSowCommandHandler(ApplicationDbContext context, ILogger<DeleteSowCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }
    public async Task<bool> Handle(DeleteSowCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Sows
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.Id && x.ProjectId.ToString() == request.ProjectId, cancellationToken: cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            _context.Sows.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred deleting sow. {exceptionMessage}", ex.Message);
            throw;
        }
    }
}



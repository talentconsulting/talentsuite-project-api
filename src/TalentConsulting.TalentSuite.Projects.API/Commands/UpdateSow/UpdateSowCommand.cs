using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using TalentConsulting.TalentSuite.Projects.Common.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Interfaces.Commands;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Projects.API.Commands.UpdateSow;

public class UpdateSowCommand : IRequest<string>, IUpdateSowCommand
{
    public UpdateSowCommand(string id, SowDto projectDto)
    {
        Id = id;
        SowDto = projectDto;
    }

    public string Id { get; }
    public SowDto SowDto { get; }
}

public class UpdateSowCommandHandler : IRequestHandler<UpdateSowCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSowCommandHandler> _logger;

    public UpdateSowCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateSowCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateSowCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Sows.FirstOrDefault(x => x.Id == request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        entity = _mapper.Map<Sow>(request.SowDto);
        ArgumentNullException.ThrowIfNull(entity);

        await _context.SaveChangesAsync(cancellationToken);

        try
        {
            //entity.ProjectId = request.SowDto.ProjectId;
            //entity.File = request.SowDto.File;
            //entity.IsChangeRequest = request.SowDto.IsChangeRequest;
            //entity.SowStartDate = request.SowDto.StartDate;
            //entity.SowEndDate = request.SowDto.EndDate;

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating sow. {exceptionMessage}", ex.Message);
            throw;
        }

        return entity.Id;
    }
}

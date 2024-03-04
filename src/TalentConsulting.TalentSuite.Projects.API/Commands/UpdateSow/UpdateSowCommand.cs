using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Projects.Common.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Interfaces.Commands;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Projects.API.Commands.UpdateSow;

public class UpdateSowCommand : IRequest<Guid>, IUpdateSowCommand
{
    public UpdateSowCommand(string id, SowDto projectDto)
    {
        Id = id;
        SowDto = projectDto;
    }

    public string Id { get; }
    public SowDto SowDto { get; }
}

public class UpdateSowCommandHandler : IRequestHandler<UpdateSowCommand, Guid>
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

    public async Task<Guid> Handle(UpdateSowCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Sows.AsNoTracking()
            .Include(x => x.Files)
            .FirstOrDefault(x => x.Id.ToString() == request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        _mapper.Map(request.SowDto, entity);

        ArgumentNullException.ThrowIfNull(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

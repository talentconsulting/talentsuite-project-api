using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
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
        var entity = _context.Sows.FirstOrDefault(x => x.Id.ToString() == request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        entity = _mapper.Map<Sow>(request.SowDto);
        ArgumentNullException.ThrowIfNull(entity);

        await _context.SaveChangesAsync(cancellationToken);

        try
        {
            entity.Files = AttachExistingSowFiles(request.SowDto.Files);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating sow. {exceptionMessage}", ex.Message);
            throw;
        }

        return entity.Id;
    }

    private ICollection<SowFile> AttachExistingSowFiles(ICollection<SowFileDto>? unSavedEntities)
    {
        var returnList = new List<SowFile>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.SowFiles.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id.ToString())).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.Find(x => x.Id.ToString() == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.Mimetype = unSavedItem.Mimetype;
                savedItem.Filename = unSavedItem.Filename;
                savedItem.Size = unSavedItem.Size;
                savedItem.SowId = new Guid(unSavedItem.SowId);
                savedItem.File = unSavedItem.File;
            }

            returnList.Add(savedItem ?? _mapper.Map<SowFile>(unSavedItem));
        }

        return returnList;
    }
}

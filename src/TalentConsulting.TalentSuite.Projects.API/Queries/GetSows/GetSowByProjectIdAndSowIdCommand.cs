using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Projects.Common.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Entities;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Projects.API.Queries.GetSows;

public class GetSowByProjectIdAndSowIdCommand : IRequest<SowDto>
{
    public GetSowByProjectIdAndSowIdCommand(string projectId, string id)
    {
        Id = id;
        ProjectId = projectId;
    }

    public string Id { get; }
    public string ProjectId { get; }

}

public class GetSowByProjectIdAndSowIdCommandHandler : IRequestHandler<GetSowByProjectIdAndSowIdCommand, SowDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSowByProjectIdAndSowIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<SowDto> Handle(GetSowByProjectIdAndSowIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Sows
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.Id && x.ProjectId.ToString() == request.ProjectId, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        var result = _mapper.Map<SowDto>(entity);

        return result;
    }
}


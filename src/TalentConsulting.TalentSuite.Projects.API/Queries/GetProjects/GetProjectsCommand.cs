using Ardalis.GuardClauses;
using MediatR;
using TalentConsulting.TalentSuite.Projects.Common.Entities;
using TalentConsulting.TalentSuite.Projects.Common;
using TalentConsulting.TalentSuite.Projects.Core.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Helpers;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace TalentConsulting.TalentSuite.Projects.API.Queries.GetProjects;

public class GetProjectsCommand : IRequest<PaginatedList<ProjectDto>>
{
    public GetProjectsCommand(int? pageNumber, int? pageSize)
    {
        PageNumber = pageNumber != null ? pageNumber.Value : 1;
        PageSize = pageSize != null ? pageSize.Value : 1;
    }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetProjectsCommandHandler : IRequestHandler<GetProjectsCommand, PaginatedList<ProjectDto>>
{
    private readonly ApplicationDbContext _context;

    public GetProjectsCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedList<ProjectDto>> Handle(GetProjectsCommand request, CancellationToken cancellationToken)
    {
        var entities = _context.Projects
            .Include(x => x.ClientProjects)
            .Include(x => x.Contacts)
            .Include(x => x.Reports)
            .ThenInclude(x => x.Risks)
            .Include(x => x.Sows);

        if (entities == null)
        {
            throw new NotFoundException(nameof(Project), "Projects");
        }

        var filteredProjects = await entities.Select(x => EntityToDtoHelper.ProjectDtoToProjectDto(x)).ToListAsync();

        if (request != null)
        {
            var pageList = filteredProjects.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var result = new PaginatedList<ProjectDto>(pageList, filteredProjects.Count, request.PageNumber, request.PageSize);

            return result;
        }

        return new PaginatedList<ProjectDto>(filteredProjects, filteredProjects.Count, 1, 10);


    }
}
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using TalentConsulting.TalentSuite.Projects.Common.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Interfaces.Commands;
using TalentConsulting.TalentSuite.Projects.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Projects.API.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest<string>, IUpdateProjectCommand
{
    public UpdateProjectCommand(string id, ProjectDto projectDto)
    {
        Id = id;
        ProjectDto = projectDto;
    }

    public string Id { get; }
    public ProjectDto ProjectDto { get; }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProjectCommandHandler> _logger;
    public UpdateProjectCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateProjectCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }
    public async Task<string> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Projects.FirstOrDefault(x => x.Id.ToString() == request.Id);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        try
        {
            entity.ContactNumber = request.ProjectDto.ContactNumber;
            entity.Name = request.ProjectDto.Name;
            entity.Reference = request.ProjectDto.Reference;
            entity.StartDate = request.ProjectDto.StartDate;
            entity.EndDate = request.ProjectDto.EndDate;

            entity.ClientProjects = AttachExistingClientProjects(request.ProjectDto.ClientProjects);
            entity.Contacts = AttachExistingContacts(request.ProjectDto.Contacts);
            entity.Reports = AttachExistingReports(request.ProjectDto.Reports);
            entity.Sows = AttachExistingSows(request.ProjectDto.Sows);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating project. {exceptionMessage}", ex.Message);
            throw;
        }

        if (request is not null && request.ProjectDto is not null)
            return request.ProjectDto.Id;
        else
            return string.Empty;
    }

    private ICollection<ClientProject> AttachExistingClientProjects(ICollection<ClientProjectDto>? unSavedEntities)
    {
        var returnList = new List<ClientProject>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.ClientProjects.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id.ToString())).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.Find(x => x.Id.ToString() == unSavedItem.Id);

            if (savedItem is not null)
            {
                if (!Guid.TryParse(unSavedItem.ProjectId, out Guid projectId))
                {
                    throw new ArgumentException("Invalid Guid for unSavedItem.ProjectId");
                }
                if (!Guid.TryParse(unSavedItem.ClientId, out Guid clientId))
                {
                    throw new ArgumentException("Invalid Guid for unSavedItem.ClientId");
                }
                savedItem.ClientId = clientId;
                savedItem.ProjectId = projectId;

            }

            returnList.Add(savedItem ?? _mapper.Map<ClientProject>(unSavedItem));

        }

        return returnList;
    }

    private ICollection<Contact> AttachExistingContacts(ICollection<ContactDto>? unSavedEntities)
    {
        var returnList = new List<Contact>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Contacts.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id.ToString())).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.Find(x => x.Id.ToString() == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.Firstname = unSavedItem.Firstname;
                savedItem.Email = unSavedItem.Email;
                savedItem.ReceivesReport = unSavedItem.ReceivesReport;
                if (!Guid.TryParse(unSavedItem.ProjectId, out Guid projectId))
                {
                    throw new ArgumentException("Invalid Guid for unSavedItem.ProjectId");
                }
                savedItem.ProjectId = projectId;
            }

            returnList.Add(savedItem ?? _mapper.Map<Contact>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<Report> AttachExistingReports(ICollection<ReportDto>? unSavedEntities)
    {
        var returnList = new List<Report>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Reports.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id.ToString())).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.Find(x => x.Id.ToString() == unSavedItem.Id);

            if (savedItem is not null)
            {
                savedItem.PlannedTasks = unSavedItem.PlannedTasks;
                savedItem.CompletedTasks = unSavedItem.CompletedTasks;
                savedItem.Weeknumber = unSavedItem.Weeknumber;
                if (!Guid.TryParse(unSavedItem.ProjectId, out Guid projectId))
                {
                    throw new ArgumentException("Invalid Guid for unSavedItem.ProjectId");
                }
                savedItem.ProjectId = projectId;
                savedItem.SubmissionDate = unSavedItem.SubmissionDate;
                if (!Guid.TryParse(unSavedItem.UserId, out Guid userId))
                {
                    throw new ArgumentException("Invalid Guid for unSavedItem.UserId");
                }
                savedItem.UserId = userId;
                savedItem.Risks = AttachExistingRisks(unSavedItem.Risks);
            }

            var item = savedItem ?? _mapper.Map<Report>(unSavedItem);

            if (savedItem is null)
            {
                var risks = AttachExistingRisks(unSavedItem.Risks);
                item.Risks = risks;
            }
                

            returnList.Add(savedItem ?? _mapper.Map<Report>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<Risk> AttachExistingRisks(ICollection<RiskDto>? unSavedEntities)
    {
        var returnList = new List<Risk>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Risks.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id.ToString())).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.Find(x => x.Id.ToString() == unSavedItem.Id);

            if (savedItem is not null)
            {
                if (!Guid.TryParse(unSavedItem.ReportId, out Guid reportId))
                {
                    throw new ArgumentException("Invalid Guid for unSavedItem.ReportId");
                }
                savedItem.ReportId = reportId;
                savedItem.RiskDetails = unSavedItem.RiskDetails;
                savedItem.RiskMitigation = unSavedItem.RiskMitigation;
                savedItem.RagStatus = unSavedItem.RagStatus;
            }

            returnList.Add(savedItem ?? _mapper.Map<Risk>(unSavedItem));
        }

        return returnList;
    }

    private ICollection<Sow> AttachExistingSows(ICollection<SowDto>? unSavedEntities)
    {
        var returnList = new List<Sow>();

        if (unSavedEntities is null || !unSavedEntities.Any())
            return returnList;

        var existing = _context.Sows.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id.ToString())).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.Find(x => x.Id.ToString() == unSavedItem.Id);

            if (savedItem is not null)
            {
                if (!Guid.TryParse(unSavedItem.ProjectId, out Guid projectId))
                {
                    throw new ArgumentException("Invalid Guid for unSavedItem.ProjectId");
                }
                savedItem.ProjectId = projectId;
                savedItem.Files = AttachExistingSowFiles(unSavedItem.Files);
                savedItem.IsChangeRequest = unSavedItem.IsChangeRequest;
                savedItem.SowStartDate = unSavedItem.StartDate;
                savedItem.SowEndDate = unSavedItem.EndDate;
            }

            returnList.Add(savedItem ?? _mapper.Map<Sow>(unSavedItem));
        }

        return returnList;
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
                savedItem.SowId = unSavedItem.SowId;
                savedItem.File = unSavedItem.File;
            }

            returnList.Add(savedItem ?? _mapper.Map<SowFile>(unSavedItem));
        }

        return returnList;
    }
}

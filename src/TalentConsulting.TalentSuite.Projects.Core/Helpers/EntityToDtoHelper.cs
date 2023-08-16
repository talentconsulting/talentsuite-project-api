using Ardalis.Specification;
using TalentConsulting.TalentSuite.Projects.Common.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Core.Helpers;

public static class EntityToDtoHelper
{
    public static ProjectDto ProjectDtoToProjectDto(Project entity)
    {
        return new ProjectDto(
             id: entity.Id,
          contactNumber: entity.ContactNumber,
          name: entity.Name,
          reference: entity.Reference,
          startDate: entity.StartDate,
          endDate: entity.EndDate,
          clientProjects: entity.ClientProjects.Select(clientProject => new ClientProjectDto(clientProject.Id, clientProject.ClientId, clientProject.ProjectId)).ToList(),
          contacts: entity.Contacts.Select(contact => new ContactDto(contact.Id, contact.Firstname, contact.Email, contact.ReceivesReport, contact.ProjectId)).ToList(),
          reports: entity.Reports.Select(report => new ReportDto(report.Id, (report.Created != null) ? report.Created.Value : DateTime.UtcNow, report.PlannedTasks, report.CompletedTasks, report.Weeknumber, report.SubmissionDate, report.ProjectId, report.UserId, GetRisks(report.Risks))).ToList(),
          sows: entity.Sows.Select(sow => new SowDto(sow.Id, (sow.Created != null) ? sow.Created.Value : DateTime.UtcNow,
          GetSows(sow.Files), 
          sow.IsChangeRequest, sow.SowStartDate, sow.SowEndDate, sow.ProjectId)).ToList()
          );
    }

    public static List<SowFileDto> GetSows(ICollection<SowFile> sowFiles)
    {
        List<SowFileDto> list = new List<SowFileDto>();
        foreach (SowFile file in sowFiles)
        {
            list.Add(new SowFileDto
            {
                Id = "bce1bb9c-36aa-4f63-9cba-d8b435a79637",
                File = file.File,
                Filename = file.Filename,
                Size = file.Size,
                SowId = file.SowId,
                Mimetype = file.Mimetype
            });
        }
        return list;
    }
    public static List<ReportDto> GetReports(ICollection<Report> reports)
    {
        return reports.Select(x => new ReportDto(x.Id, (x.Created != null) ? x.Created.Value : DateTime.UtcNow, x.PlannedTasks, x.CompletedTasks, x.Weeknumber, x.SubmissionDate, x.ProjectId, x.UserId, GetRisks(x.Risks))).ToList();
    }

    public static List<RiskDto> GetRisks(ICollection<Risk> risks)
    {
        return risks.Select(x => new RiskDto(x.Id, x.ReportId, x.RiskDetails, x.RiskMitigation, x.RagStatus)).ToList();
    }

    public static List<ClientProjectDto> GetClientProjects(ICollection<ClientProject> clientProjects)
    {
        return clientProjects.Select(x => new ClientProjectDto(x.Id, x.ClientId, x.ProjectId)).ToList();
    }
}

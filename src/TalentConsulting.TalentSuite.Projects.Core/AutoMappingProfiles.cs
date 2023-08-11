﻿using AutoMapper;
using TalentConsulting.TalentSuite.Projects.Common.Entities;
using TalentConsulting.TalentSuite.Projects.Core.Entities;

namespace TalentConsulting.TalentSuite.Projects.Core;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<AuditDto, Audit>().ReverseMap();
        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<ClientDto, Client>().ReverseMap();
        CreateMap<ClientProjectDto, ClientProject>().ReverseMap();
        CreateMap<ProjectDto, Project>().ReverseMap();
        CreateMap<ProjectRoleDto, ProjectRole>().ReverseMap();
        CreateMap<RecipientDto, Recipient>().ReverseMap();
        CreateMap<ReportDto, Report>().ReverseMap();
        CreateMap<RiskDto, Risk>().ReverseMap();
        CreateMap<SowDto, Sow>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<UserGroupDto, UserGroup>().ReverseMap();
        CreateMap<UserProjectRoleDto, UserProjectRoleDto>().ReverseMap();
    }
}

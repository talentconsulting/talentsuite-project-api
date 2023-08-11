﻿using System.ComponentModel.DataAnnotations.Schema;
using TalentConsulting.TalentSuite.Projects.Common;
using TalentConsulting.TalentSuite.Projects.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Projects.Core.Entities;

[Table("clientprojects")]
public class ClientProject : EntityBase<string>, IAggregateRoot
{
    private ClientProject() { }

    public ClientProject(string id, string clientid, string projectid)
    {
        Id = id;
        ClientId = clientid;
        ProjectId = projectid;
    }

    public string ClientId { get; set; } = null!;
    public string ProjectId { get; set; } = null!;
#if ADD_ENTITY_NAV
    public virtual Client Client { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
#endif

}
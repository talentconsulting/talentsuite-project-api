﻿using System.ComponentModel.DataAnnotations.Schema;
using TalentConsulting.TalentSuite.Projects.Common;
using TalentConsulting.TalentSuite.Projects.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Projects.Core.Entities;

[Table("sows")]
public class Sow : EntityBase<string>, IAggregateRoot
{
    private Sow() { }

    public Sow(string id, DateTime created, byte[] file, bool ischangerequest, DateTime sowstartdate, DateTime sowenddate, string projectid)
    {
        Id = id;
        Created = created;
        File = file;
        IsChangeRequest = ischangerequest;
        SowStartDate = sowstartdate;
        SowEndDate = sowenddate;
        ProjectId = projectid;
    }

    //ToDo Add Sow Files table
    public byte[] File { get; set; } = null!;
    public bool IsChangeRequest { get; set; }
    public DateTime SowStartDate { get; set; }
    public DateTime SowEndDate { get; set; }
    public string ProjectId { get; set; } = null!;
#if ADD_ENTITY_NAV
    public virtual Project Project { get; set; } = null!;
#endif

}
using TalentConsulting.TalentSuite.Projects.Common;
using TalentConsulting.TalentSuite.Projects.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Projects.Core.Entities;

public class Audit : EntityBase<string>, IAggregateRoot
{
    private Audit() { }

    public Audit(string id, string detail, string userid)
    {
        Id = id;
        Detail = detail;
        UserId = userid;
    }

    public string Detail { get; init; } = null!;
    public string UserId { get; init; } = null!;
}
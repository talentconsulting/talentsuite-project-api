using System.ComponentModel.DataAnnotations.Schema;
using TalentConsulting.TalentSuite.Projects.Common;
using TalentConsulting.TalentSuite.Projects.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Projects.Core.Entities;

[Table("usergroups")]
public class UserGroup : EntityBase<string>, IAggregateRoot
{
    private UserGroup() { }

    public UserGroup(string id, string name, bool receivesreports)
    {
        Id = id;
        Name = name;
        ReceivesReports = receivesreports;
    }
    public string Name { get; init; } = null!;
    public bool? ReceivesReports { get; init; } = null!;
    public virtual ICollection<User> Users { get; } = new List<User>();

}
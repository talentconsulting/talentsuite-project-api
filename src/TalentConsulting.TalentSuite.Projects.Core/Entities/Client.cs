using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TalentConsulting.TalentSuite.Projects.Common;
using TalentConsulting.TalentSuite.Projects.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Projects.Core.Entities;

[Table("cients")]
public class Client : EntityBase<Guid>, IAggregateRoot
{
    private Client() { }

    public Client(Guid id, string name, string contactname, string contactemail, ICollection<ClientProject> clientProjects)
    {
        Id = id;
        Name = name;
        ContactName = contactname;
        ContactEmail = contactemail;
        ClientProjects = clientProjects;
    }

    public string Name { get; set; } = null!;
    public string ContactName { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public virtual ICollection<ClientProject> ClientProjects { get; set; } = new List<ClientProject>();
}
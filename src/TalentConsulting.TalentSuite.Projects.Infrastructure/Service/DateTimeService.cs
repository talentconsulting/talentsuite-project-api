using TalentConsulting.TalentSuite.Projects.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Projects.Infrastructure.Service;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}


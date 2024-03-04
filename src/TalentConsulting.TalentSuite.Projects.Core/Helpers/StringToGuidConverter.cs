using AutoMapper;

namespace TalentConsulting.TalentSuite.Projects.Core.Helpers;

public class StringToGuidConverter : ITypeConverter<string, Guid>
{
    public Guid Convert(string source, Guid destination, ResolutionContext context)
    {
        return new Guid(source);
    }
}

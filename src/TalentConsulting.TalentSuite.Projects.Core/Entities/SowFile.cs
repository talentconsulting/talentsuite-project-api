using TalentConsulting.TalentSuite.Projects.Common;

namespace TalentConsulting.TalentSuite.Projects.Core.Entities;

public class SowFile : EntityBase<string>
{
    public required string Mimetype { get; set; }
    public required string Filename { get; set; }
    public required int Size { get; set; }
    public required string SowId { get; set; }
    public required byte[] File { get; set; }
}

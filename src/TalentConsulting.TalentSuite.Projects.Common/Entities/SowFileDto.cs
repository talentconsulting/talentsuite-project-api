namespace TalentConsulting.TalentSuite.Projects.Common.Entities;

public class SowFileDto
{
    public required string Mimetype { get; set; }
    public required string Filename { get; set; }
    public required int Size { get; set; }
    public required string SowId { get; set; }
    public required byte[] File { get; set; }
}

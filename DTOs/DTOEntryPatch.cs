namespace Temachti.Api.DTOs;

public class DTOEntryPatch
{
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public string Tags { get; set; }
    public string UrlCover { get; set; }
    public int TechnologyId { get; set; }
}
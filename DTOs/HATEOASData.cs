namespace Temachti.Api.DTOs;

public class HATEOASData
{
    public string Link { get; private set; }    
    public string Description { get; private set; }
    public string Method { get; private set; }

    public HATEOASData(string link, string description, string method)
    {
        this.Link = link;
        this.Description = description;
        this.Method = method;
    }
}
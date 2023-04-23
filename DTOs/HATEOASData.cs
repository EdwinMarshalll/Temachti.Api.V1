namespace Temachti.Api.DTOs;

public class HATEOASData
{
    public string Href { get; private set; }
    public string Rel { get; private set; }
    public string Action { get; private set; }

    public HATEOASData(string href, string rel, string action)
    {
        this.Href = href;
        this.Rel = rel;
        this.Action = action;
    }
}
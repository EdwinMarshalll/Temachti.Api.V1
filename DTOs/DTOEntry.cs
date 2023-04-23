namespace Temachti.Api.DTOs;

public class DTOEntry : Resource
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public double Rating { get; set; }
    public int Views { get; set; }
    public string UrlCover { get; set; }
    public string Tags { get; set; }
}
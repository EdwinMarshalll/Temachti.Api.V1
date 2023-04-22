using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.DTOs;

public class DTOEntryCreate
{
    [Required]
    [StringLength(maximumLength: 20)]
    public string Code { get; set; }

    [Required]
    [StringLength(maximumLength: 150)]
    public string Title { get; set; }

    [Required]
    [StringLength(maximumLength: 300)]
    public string Description { get; set; }

    [Required]
    public string Content { get; set; }

    [Url]
    public string UrlCover { get; set; }

    public string Tags { get; set; }
    public int TechnologyId { get; set; }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Temachti.Api.Entities;

public class Entry
{
    public int Id { get; set; }

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

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public double Rating { get; set; }
    public int Views { get; set; }

    public string Tags { get; set; }

    [Url]
    public string UrlCover { get; set; }

    public int TechnologyId { get; set; }
    public string UserId { get; set; }


    public Technology Technology { get; set; }
    public IdentityUser User { get; set; }
}
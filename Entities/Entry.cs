using Microsoft.AspNetCore.Identity;

namespace Temachti.Api.Entities;

public class Entry
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string Rating { get; set; }
    public string Tags { get; set; }
    public string TechnologyId { get; set; }
    public string UserId { get; set; }

    
    public Technology Technology { get; set; }
    public IdentityUser User { get; set; }
}
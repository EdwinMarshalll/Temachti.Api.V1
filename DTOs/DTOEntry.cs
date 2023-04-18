using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Temachti.Api.Entities;

namespace Temachti.Api.DTOs;

public class DTOEntry
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public double Rating { get; set; }
    public string Tags { get; set; }
}
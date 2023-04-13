using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.DTOs;

public class DTOUserCredentials
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
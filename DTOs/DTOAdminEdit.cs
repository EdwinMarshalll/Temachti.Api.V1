using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.DTOs;

public class DTOAdminEdit
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
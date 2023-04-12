using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.DTOs;

public class DTOUserCreate
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Nombre(s)")]
    [StringLength(maximumLength: 50)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Apellido(s)")]
    [StringLength(maximumLength: 50)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Usuario")]
    [StringLength(maximumLength: 20)]
    public string Nickname { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Correo")]
    [StringLength(maximumLength: 100)]
    [EmailAddress(ErrorMessage = "El campo {0} no es valido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Contrasena")]
    public string Password { get; set; }
}
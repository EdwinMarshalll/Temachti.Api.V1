using System.ComponentModel.DataAnnotations;
using Temachti.Api.Validations;

namespace Temachti.Api.Entities;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Nombre(s)")]
    [StringLength(maximumLength: 50, ErrorMessage = "La longitud debe ser de maxima de {0}")]
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

    [Display(Name = "Telefono")]
    [StringLength(maximumLength: 15)]
    [Phone(ErrorMessage = "El campo {0} no es valido")]
    public string Phone { get; set; }

    [Display(Name = "Biografia")]
    [StringLength(maximumLength: 500)]
    // TODO: Ejemplo de validacion personalizada
    // [FirstCapitalLetter]
    public string Biography { get; set; }

    [Display(Name = "Fecha de creacion")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Fecha de modificacion")]
    public DateTime ModifiedAt { get; set; }

    [Display(Name = "Rol")]
    public int RoleId { get; set; }
    public Role Role { get; set; }

    [Display(Name = "Membresia")]
    public int MembershipId { get; set; }

    [Url(ErrorMessage = "Ingresa una url validada")]
    public string UrlPhoto { get; set; }

}
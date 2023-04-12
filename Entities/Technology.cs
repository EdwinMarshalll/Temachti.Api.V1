using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.Entities;

public class Technology
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 15)]
    [Display(Name = "Contrasena")]
    public string Code { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 30)]
    [Display(Name = "Nombre")]
    public string Name { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 100)]
    [Display(Name = "Descripcion")]
    public string Description { get; set; }

    [Display(Name = "Fecha de creacion")]
    public DateTime CreatedAt { get; set; }
}
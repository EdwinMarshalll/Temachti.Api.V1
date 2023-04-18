using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.DTOs;

public class DTOTechnologyCreate
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 20)]
    [Display(Name = "Codigo")]
    public string Code { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 30)]
    [Display(Name = "Nombre")]
    public string Name { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 100)]
    [Display(Name = "Descripcion")]
    public string Description { get; set; }
}
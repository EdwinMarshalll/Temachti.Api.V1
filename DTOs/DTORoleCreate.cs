using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.DTOs;

public class DTORoleCreate 
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Codigo")]
    [StringLength(maximumLength: 10, ErrorMessage = "La longitud maxima del campo {0} es de {1}")]
    public string Code { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Nombre")]
    [StringLength(maximumLength: 14, ErrorMessage = "La longitud maxima del campo {0} es de {1}")]
    public string Name { get; set; }
   
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 100, ErrorMessage = "La longitud maxima del campo {0} es de {1}")]
    [Display(Name = "Nombre")]
    public string Description { get; set; }
}
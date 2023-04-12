using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.Entities;

public class Role : IValidatableObject
{
    public int Id { get; set; }

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

    [Display(Name = "Habilitado")]
    public bool IsActive {get; set;}


    /// <summary>
    /// aqui puede ir validaciones a nivel modelo que se ejecutaran despues de pasar las validaciones de Atributos
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(!string.IsNullOrEmpty(Name)){
            if(Name.Equals("Algo"))
            {
                // con yield, se inserta el error en el IEnumerable
                yield return new ValidationResult("El campo no puede ser Algo",
                new string[] { nameof(Name)});
            }
        }
    }
}
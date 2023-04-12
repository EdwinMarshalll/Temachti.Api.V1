using System.ComponentModel.DataAnnotations;

namespace Temachti.Api.Validations;

public class FirstCapitalLetterAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if(value is null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var fisrtChar = value.ToString()[0].ToString();

        if( fisrtChar != fisrtChar.ToUpper())
        {
            return new ValidationResult("La primera letra debe ser mayuscula");
        }

        return ValidationResult.Success;
    }
}
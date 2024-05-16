using IbanNet;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace AtmiraPayNet.Shared.ValidationAttributes
{
    public class IbanValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var iban = (string?)value;

            if (string.IsNullOrEmpty(iban))
            {
                return ValidationResult.Success;
            }

            var ibanValidator = new IbanValidator();
            var validationResult = ibanValidator.Validate(iban.Replace(" ", ""));

            if (validationResult.IsValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("IBAN inválido");
            }
        }
    }
}

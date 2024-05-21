using AtmiraPayNet.Shared.DTO;
using System.ComponentModel.DataAnnotations;

namespace AtmiraPayNet.Shared.ValidationAttributes
{
    public class CCA2CountryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = (PaymentDTO)validationContext.ObjectInstance;

            if (model != null)
            {
                string? ibanCCA2 = validationContext.DisplayName switch
                {
                    "SourceBankCountry" => model.SourceIBAN?.Length >= 2 ? model.SourceIBAN[..2] : null,
                    "DestinationBankCountry" => model.DestinationIBAN?.Length >= 2 ? model.DestinationIBAN[..2] : null,
                    "IntermediaryBankCountry" => model.IntermediaryIBAN?.Length >= 2 ? model.IntermediaryIBAN[..2] : null,
                    _ => null
                };

                string? countryCCA2 = validationContext.DisplayName switch
                {
                    "SourceBankCountry" => model.SourceBankCCA2,
                    "DestinationBankCountry" => model.DestinationBankCCA2,
                    "IntermediaryBankCountry" => model.IntermediaryBankCCA2,
                    _ => null
                };

                if (!string.IsNullOrEmpty(ibanCCA2) &&
                    !string.IsNullOrEmpty(countryCCA2) &&
                    countryCCA2 != ibanCCA2)
                {
                    return new ValidationResult("El país no concuerda con el IBAN");
                }
            }

            return ValidationResult.Success!;
        }
    }
}

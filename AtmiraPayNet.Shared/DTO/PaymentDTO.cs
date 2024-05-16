using AtmiraPayNet.Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace AtmiraPayNet.Shared.DTO
{
    public class PaymentDTO
    {
        [Required(ErrorMessage = "IBAN requerido")]
        [StringLength(29, MinimumLength = 29, ErrorMessage = "IBAN inválido")]
        public string? SourceIBAN { get; set; }

        [Required(ErrorMessage = "Banco requerido")]
        public string? SourceBankName { get; set; }

        [Required(ErrorMessage = "País requerido")]
        public string? SourceBankCountry { get; set; }

        [Required(ErrorMessage = "Código postal requerido")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Calle requerida")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Número requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Número inválido")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Ciudad requerida")]
        public string? City { get; set; }

        [Required(ErrorMessage = "País requerido")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Cantidad requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad inválida")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "IBAN requerido")]
        [StringLength(29, MinimumLength = 29, ErrorMessage = "IBAN inválido")]
        public string? DestinationIBAN { get; set; }

        [Required(ErrorMessage = "Banco requerido")]
        public string? DestinationBankName { get; set; }

        [Required(ErrorMessage = "País requerido")]
        public string? DestinationBankCountry { get; set; }

        [RequiredIfIBANPrefixDiffers]
        public string? IntermediaryIBAN { get; set; }

        [RequiredIfPropertyPrefixDiffers("Banco requerido")]
        public string? IntermediaryBankName { get; set; }

        [RequiredIfPropertyPrefixDiffers("País requerido")]
        public string? IntermediaryBankCountry { get; set; }

        public Status Status { get; set; }
    }

    public class RequiredIfIBANPrefixDiffersAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (PaymentDTO)validationContext.ObjectInstance;

            if (model != null)
            {
                var sourceIBAN = model.SourceIBAN;
                var destinationIBAN = model.DestinationIBAN;
                var intermediaryIBAN = (string?)value;

                if (!string.IsNullOrEmpty(sourceIBAN) &&
                    !string.IsNullOrEmpty(destinationIBAN) &&
                    sourceIBAN.Length >= 2 &&
                    destinationIBAN.Length >= 2 &&
                    sourceIBAN[..2] != destinationIBAN[..2])
                {
                    if (string.IsNullOrEmpty(intermediaryIBAN))
                    {
                        return new ValidationResult("IBAN requerido");
                    }
                    else if (model.IntermediaryIBAN!.Length != 29)
                    {
                        return new ValidationResult("IBAN inválido");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

    public class RequiredIfPropertyPrefixDiffersAttribute(string errorMessage) : ValidationAttribute
    {
        public new string ErrorMessage { get; set; } = errorMessage;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (PaymentDTO)validationContext.ObjectInstance;

            if (model != null)
            {
                var sourceIBAN = model.SourceIBAN;
                var destinationIBAN = model.DestinationIBAN;
                var property = (string?)value;

                if (!string.IsNullOrEmpty(sourceIBAN) &&
                    !string.IsNullOrEmpty(destinationIBAN) &&
                    sourceIBAN.Length >= 2 &&
                    destinationIBAN.Length >= 2 &&
                    sourceIBAN[..2] != destinationIBAN[..2] &&
                    string.IsNullOrEmpty(property))
                {
                    return new ValidationResult(ErrorMessage);

                }
            }

            return ValidationResult.Success;
        }
    }

    
}

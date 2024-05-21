using AtmiraPayNet.Shared.DTO;
using System.ComponentModel.DataAnnotations;

namespace AtmiraPayNet.Shared.ValidationAttributes
{
    public class RequiredIfIBANPrefixDiffersAttribute : ValidationAttribute
    {
        public RequiredIfIBANPrefixDiffersAttribute() { }

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
                    sourceIBAN[..2] != destinationIBAN[..2] &&
                    string.IsNullOrEmpty(intermediaryIBAN))
                {
                    return new ValidationResult(ErrorMessage);
                }

            }
            return ValidationResult.Success;
        }
    }
}

using AtmiraPayNet.Shared.DTO;
using System.ComponentModel.DataAnnotations;

namespace AtmiraPayNet.Shared.ValidationAttributes
{
    public class SameIBANValidationAttribute : ValidationAttribute
    {
        public SameIBANValidationAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (PaymentDTO)validationContext.ObjectInstance;

            if (model != null)
            {
                var sourceIBAN = model.SourceIBAN ?? "Source";
                var destinationIBAN = model.DestinationIBAN ?? "Destination";
                var intermediaryIBAN = model.IntermediaryIBAN ?? "Intermediary";

                if (sourceIBAN == destinationIBAN || sourceIBAN == intermediaryIBAN || destinationIBAN == intermediaryIBAN)
                {
                    return new ValidationResult("Los IBANs no pueden ser iguales");
                }
            }
            return ValidationResult.Success;
        }
    }
}

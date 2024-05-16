using AtmiraPayNet.Shared.Utils;
using AtmiraPayNet.Shared.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace AtmiraPayNet.Shared.DTO
{
    public class PaymentDTO
    {
        [Required(ErrorMessage = "IBAN requerido")]
        [IbanValidation]
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
        [IbanValidation]
        public string? DestinationIBAN { get; set; }

        [Required(ErrorMessage = "Banco requerido")]
        public string? DestinationBankName { get; set; }

        [Required(ErrorMessage = "País requerido")]
        public string? DestinationBankCountry { get; set; }

        [RequiredIfIBANPrefixDiffers(ErrorMessage = "IBAN requerido")]
        [IbanValidation]
        public string? IntermediaryIBAN { get; set; }

        [RequiredIfIBANPrefixDiffers(ErrorMessage = "Banco requerido")]
        public string? IntermediaryBankName { get; set; }

        [RequiredIfIBANPrefixDiffers(ErrorMessage = "País requerido")]
        public string? IntermediaryBankCountry { get; set; }

        public Status Status { get; set; }
    }
}

using AtmiraPayNet.Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace AtmiraPayNet.Shared.DTO
{
    public class PaymentDTO
    {
        [Required(ErrorMessage = "El IBAN de la cuenta de origen es requerido")]
        [RegularExpression(@"^[A-Z]{2}\d{2}[A-Z\d]{4}\d{7}([A-Z\d]?){0,16}$", ErrorMessage = "El formato del IBAN no es válido")]
        public string? SourceIBAN { get; set; }

        [Required(ErrorMessage = "El nombre del banco de origen es requerido")]
        public string? SourceBankName { get; set; }

        [Required(ErrorMessage = "El país del banco de origen es requerido")]
        public string? SourceBankCountry { get; set; }

        [Required(ErrorMessage = "El nombre del titular de la cuenta de origen es requerido")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "La calle del titular de la cuenta de origen es requerida")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "El número de la calle del titular de la cuenta de origen es requerido")]
        public int Number { get; set; }

        [Required(ErrorMessage = "La cantidad de la transacción es requerida")]
        public float Amount { get; set; }

        [Required(ErrorMessage = "El IBAN de la cuenta de destino es requerido")]
        public string? DestinationIBAN { get; set; }

        [Required(ErrorMessage = "El nombre del banco de destino es requerido")]
        public string? DestinationBankName { get; set; }

        [Required(ErrorMessage = "El país del banco de destino es requerido")]
        public string? DestinationBankCountry { get; set; }

        public string? IntermediaryIBAN { get; set; }
        public string? IntermediaryBankName { get; set; }
        public string? IntermediaryBankCountry { get; set; }

        public Status Status { get; set; }

        public string? ErrorSourceBankCountry { get; set; }
        public string? ErrorDestinationBankCountry { get; set; }
    }
}

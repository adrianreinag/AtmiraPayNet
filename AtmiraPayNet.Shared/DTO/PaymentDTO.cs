using AtmiraPayNet.Shared.Utils;

namespace AtmiraPayNet.Shared.DTO
{
    public class PaymentDTO
    {
        public Guid? Id { get; set; }
        required public string SourceIBAN { get; set; }
        required public string SourceBankName { get; set; }
        required public string SourceBankCountry { get; set; }
        required public string PostalCode { get; set; }
        required public string Street { get; set; }
        required public int Number { get; set; }
        required public float Amount { get; set; }
        required public string DestinationIBAN { get; set; }
        required public string DestinationBankName { get; set; }
        required public string DestinationBankCountry { get; set; }
        public string? IntermediaryIBAN { get; set; }
        public string? IntermediaryBankName { get; set; }
        public string? IntermediaryBankCountry { get; set; }
        required public Status Status { get; set; }
    }
}

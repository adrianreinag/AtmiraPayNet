namespace AtmiraPayNet.RPA.Models
{
    internal class PaymentModel
    {
        public required string SourceIBAN { get; set; }
        public required string SourceBankName { get; set; }
        public required string SourceBankCountry { get; set; }
        public required string PostalCode { get; set; }
        public required string Street { get; set; }
        public required string Number { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string Amount { get; set; }
        public required string DestinationIBAN { get; set; }
        public required string DestinationBankName { get; set; }
        public required string DestinationBankCountry { get; set; }
        public string? IntermediaryIBAN { get; set; }
        public string? IntermediaryBankName { get; set; }
        public string? IntermediaryBankCountry { get; set; }
    }
}

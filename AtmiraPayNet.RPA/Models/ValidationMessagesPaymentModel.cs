namespace AtmiraPayNet.RPA.Models
{
    internal class ValidationMessagesPaymentModel
    {
        public string? ValidationMessageSourceIBAN { get; set; }
        public string? ValidationMessageSourceBankName { get; set; }
        public string? ValidationMessageSourceBankCountry { get; set; }
        public string? ValidationMessagePostalCode { get; set; }
        public string? ValidationMessageStreet { get; set; }
        public string? ValidationMessageNumber { get; set; }
        public string? ValidationMessageCity { get; set; }
        public string? ValidationMessageCountry { get; set; }
        public string? ValidationMessageAmount { get; set; }
        public string? ValidationMessageDestinationIBAN { get; set; }
        public string? ValidationMessageDestinationBankName { get; set; }
        public string? ValidationMessageDestinationBankCountry { get; set; }
        public string? ValidationMessageIntermediaryIBAN { get; set; }
        public string? ValidationMessageIntermediaryBankName { get; set; }
        public string? ValidationMessageIntermediaryBankCountry { get; set; }

        public bool IsOkey()
        {
            return string.IsNullOrWhiteSpace(ValidationMessageSourceIBAN) &&
                   string.IsNullOrWhiteSpace(ValidationMessageSourceBankName) &&
                   string.IsNullOrWhiteSpace(ValidationMessageSourceBankCountry) &&
                   string.IsNullOrWhiteSpace(ValidationMessagePostalCode) &&
                   string.IsNullOrWhiteSpace(ValidationMessageStreet) &&
                   string.IsNullOrWhiteSpace(ValidationMessageNumber) &&
                   string.IsNullOrWhiteSpace(ValidationMessageCity) &&
                   string.IsNullOrWhiteSpace(ValidationMessageCountry) &&
                   string.IsNullOrWhiteSpace(ValidationMessageAmount) &&
                   string.IsNullOrWhiteSpace(ValidationMessageDestinationIBAN) &&
                   string.IsNullOrWhiteSpace(ValidationMessageDestinationBankName) &&
                   string.IsNullOrWhiteSpace(ValidationMessageDestinationBankCountry) &&
                   string.IsNullOrWhiteSpace(ValidationMessageIntermediaryIBAN) &&
                   string.IsNullOrWhiteSpace(ValidationMessageIntermediaryBankName) &&
                   string.IsNullOrWhiteSpace(ValidationMessageIntermediaryBankCountry);
        }
    }
}

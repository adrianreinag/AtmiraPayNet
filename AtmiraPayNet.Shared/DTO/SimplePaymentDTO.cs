using AtmiraPayNet.Shared.Utils;

namespace AtmiraPayNet.Shared.DTO
{
    public class SimplePaymentDTO
    {
        public required Guid Id { get; set; }
        public required string SourceBankName { get; set; }
        public required string DestinationBankName { get; set; }
        public required CurrencyDTO Currency { get; set; }
        public required float Amount { get; set; }
        public required Status Status { get; set; }
    }
}

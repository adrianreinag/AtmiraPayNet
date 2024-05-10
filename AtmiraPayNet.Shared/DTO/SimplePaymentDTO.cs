using AtmiraPayNet.Shared.Utils;

namespace AtmiraPayNet.Shared.DTO
{
    public class SimplePaymentDTO
    {
        required public Guid Id { get; set; }
        required public string SourceBankName { get; set; }
        required public string DestinationBankName { get; set; }
        required public CurrencyDTO Currency { get; set; }
        required public float Amount { get; set; }
        required public Status Status { get; set; }
    }
}

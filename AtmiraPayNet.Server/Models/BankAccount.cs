namespace AtmiraPayNet.Server.Models
{
    public class BankAccount
    {
        // Primary key
        required public Guid Id { get; set; }

        // Relationships
        required public Guid BankId { get; set; }
        public Bank? Bank { get; set; }

        required public List<Payment> SourcePayments { get; set; }

        required public List<Payment> DestinationPayments { get; set; }

        required public List<Payment> IntermediaryPayments { get; set; }

        // Properties
        required public IBAN IBAN { get; set; }
    }
}

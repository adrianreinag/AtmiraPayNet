namespace AtmiraPayNet.Server.Models
{
    public class BankAccount
    {
        // Primary key
        public required Guid Id { get; set; }

        // Relationships
        public required Guid BankId { get; set; }
        public Bank? Bank { get; set; }

        public required List<Payment> SourcePayments { get; set; }

        public required List<Payment> DestinationPayments { get; set; }

        public required List<Payment> IntermediaryPayments { get; set; }

        // Properties
        public required IBAN IBAN { get; set; }
    }
}

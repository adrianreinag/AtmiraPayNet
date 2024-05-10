using AtmiraPayNet.Shared;

namespace AtmiraPayNet.Server.Models
{
    public class Payment
    {
        // Primary key
        required public Guid Id { get; set; }

        // Relationships
        required public Guid UserId { get; set; }
        public User? User { get; set; }

        required public Guid SourceAccountId { get; set; }
        public BankAccount? SourceAccount { get; set; }

        required public Guid DestinationAccountId { get; set; }
        public BankAccount? DestinationAccount { get; set; }

        public Guid? IntermediaryAccountId { get; set; }
        public BankAccount? IntermediaryAccount { get; set; }

        public PaymentLetter? PaymentLetter { get; set; }

        // Properties
        required public Address Address { get; set; }
        required public float Amount { get; set; }
        required public Status Status { get; set; }
    }
}

using AtmiraPayNet.Shared.Utils;

namespace AtmiraPayNet.Server.Models
{
    public class Payment
    {
        // Primary key
        public required Guid Id { get; set; }

        // Relationships
        public required string UserId { get; set; }
        public User? User { get; set; }

        public required Guid SourceAccountId { get; set; }
        public BankAccount? SourceAccount { get; set; }

        public required Guid DestinationAccountId { get; set; }
        public BankAccount? DestinationAccount { get; set; }

        public Guid? IntermediaryAccountId { get; set; }
        public BankAccount? IntermediaryAccount { get; set; }

        public Guid? PaymentLetterId { get; set; }
        public PaymentLetter? PaymentLetter { get; set; }

        // Properties
        public required Address Address { get; set; }
        public required int Amount { get; set; }
        public required Status Status { get; set; }
    }
}

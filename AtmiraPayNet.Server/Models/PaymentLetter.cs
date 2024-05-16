namespace AtmiraPayNet.Server.Models
{
    public class PaymentLetter
    {
        // Primary key
        public required Guid Id { get; set; }

        // Relationships
        public required Guid PaymentId { get; set; }
        public Payment? Payment { get; set; }

        // Properties
        public required string PDF { get; set; }
    }
}

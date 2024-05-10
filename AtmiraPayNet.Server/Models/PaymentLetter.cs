namespace AtmiraPayNet.Server.Models
{
    public class PaymentLetter
    {
        // Primary key
        required public Guid Id { get; set; }

        // Relationships
        required public Guid PaymentId { get; set; }
        public Payment? Payment { get; set; }

        // Properties
        required public string PDF { get; set; }
    }
}

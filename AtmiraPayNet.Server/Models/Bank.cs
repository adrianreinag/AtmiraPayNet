namespace AtmiraPayNet.Server.Models
{
    public class Bank
    {
        // Primary key
        public required Guid Id { get; set; }

        // Relationships
        public required List<BankAccount> BankAccounts { get; set; }

        // Properties
        public required string Name { get; set; }
        public required string CountryName { get; set; }
    }
}

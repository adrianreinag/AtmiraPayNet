namespace AtmiraPayNet.Server.Models
{
    public class Bank
    {
        // Primary key
        required public Guid Id { get; set; }

        // Relationships
        required public List<BankAccount> BankAccounts { get; set; }

        // Properties
        required public string Name { get; set; }
        required public string CountryName { get; set; }
    }
}

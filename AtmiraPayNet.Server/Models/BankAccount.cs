namespace AtmiraPayNet.Server.Models
{
    public class BankAccount(string iban, string bankName, string bankCountry)
    {
        public string Iban { get; set; } = iban;
        public string BankName { get; set; } = bankName;
        public string BankCountry { get; set; } = bankCountry;
    }
}

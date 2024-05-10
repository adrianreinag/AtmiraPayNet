namespace AtmiraPayNet.Server.Models
{
    public class Address(int number, string street, string country, string postalCode)
    {
        public int Number { get; set; } = number;
        public string Street { get; set; } = street;
        public string Country { get; set; } = country;
        public string PostalCode { get; set; } = postalCode;
    }
}

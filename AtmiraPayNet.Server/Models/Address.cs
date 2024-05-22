namespace AtmiraPayNet.Server.Models
{
    public class Address(int number, string street, string city, string country, string postalCode)
    {
        public int Number { get; set; } = number;
        public string Street { get; set; } = street;
        public string City { get; set; } = city;
        public string Country { get; set; } = country;
        public string PostalCode { get; set; } = postalCode;

        public override string ToString()
        {
            return $"{Number} {Street}, {City}, {Country}, {PostalCode}";
        }
    }
}

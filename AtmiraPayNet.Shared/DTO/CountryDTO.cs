using Newtonsoft.Json;

namespace AtmiraPayNet.Shared.DTO
{
    public class CountryDTO
    {
        [JsonProperty("name")]
        public required Name Name { get; set; }

        [JsonProperty("cca2")]
        public required string CCA2 { get; set; }

        [JsonProperty("currencies")]
        public required Dictionary<string, Currency> Currencies { get; set; }
    }

    public class Name
    {
        [JsonProperty("common")]
        public required string Common { get; set; }

        [JsonProperty("official")]
        public required string Official { get; set; }

        [JsonProperty("nativeName")]
        public required Dictionary<string, NativeName> NativeName { get; set; }
    }

    public class NativeName
    {
        [JsonProperty("official")]
        public required string Official { get; set; }

        [JsonProperty("common")]
        public required string Common { get; set; }
    }

    public class Currency
    {
        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("symbol")]
        public required string Symbol { get; set; }
    }
}
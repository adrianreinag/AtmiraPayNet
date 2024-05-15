using Newtonsoft.Json;

namespace AtmiraPayNet.Shared.DTO
{
    public class CountryDTO
    {
        [JsonProperty("name")]
        required public Name Name { get; set; }

        [JsonProperty("cca2")]
        required public string CCA2 { get; set; }

        [JsonProperty("currencies")]
        required public Dictionary<string, Currency> Currencies { get; set; }
    }

    public class Name
    {
        [JsonProperty("common")]
        required public string Common { get; set; }

        [JsonProperty("official")]
        required public string Official { get; set; }

        [JsonProperty("nativeName")]
        required public Dictionary<string, NativeName> NativeName { get; set; }
    }

    public class NativeName
    {
        [JsonProperty("official")]
        required public string Official { get; set; }

        [JsonProperty("common")]
        required public string Common { get; set; }
    }

    public class Currency
    {
        [JsonProperty("name")]
        required public string Name { get; set; }

        [JsonProperty("symbol")]
        required public string Symbol { get; set; }
    }
}
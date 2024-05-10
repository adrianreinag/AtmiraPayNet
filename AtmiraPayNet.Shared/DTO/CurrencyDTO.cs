using Newtonsoft.Json;

namespace AtmiraPayNet.Shared.DTO
{
    public class CurrencyDTO
    {
        [JsonProperty("name")]
        required public string Name { get; set; }

        [JsonProperty("symbol")]
        required public string Symbol { get; set; }
    }
}

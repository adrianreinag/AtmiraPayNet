using Newtonsoft.Json;

namespace AtmiraPayNet.Shared.DTO
{
    public class CurrencyDTO
    {
        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("symbol")]
        public required string Symbol { get; set; }
    }
}

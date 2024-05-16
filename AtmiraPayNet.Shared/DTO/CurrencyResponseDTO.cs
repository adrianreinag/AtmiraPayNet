using Newtonsoft.Json;

namespace AtmiraPayNet.Shared.DTO
{
    public class CurrencyResponseDTO
    {
        [JsonProperty("currencies")]
        public required Dictionary<string, CurrencyDTO> Currencies { get; set; }
    }
}

using Newtonsoft.Json;

namespace AtmiraPayNet.Shared.DTO
{
    public class CurrencyResponseDTO
    {
        [JsonProperty("currencies")]
        required public Dictionary<string, CurrencyDTO> Currencies { get; set; }
    }
}

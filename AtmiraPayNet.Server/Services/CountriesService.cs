using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Newtonsoft.Json.Linq;

namespace AtmiraPayNet.Server.Services
{
    public class CountriesService(HttpClient http) : ICountriesService
    {
        //private readonly string _countriesFilePath = "Assets/Countries.json";
        private readonly HttpClient _http = http;

        public async Task<CurrencyDTO> GetCurrencyByCountryName(string countryName)
        {
            HttpResponseMessage response = await _http.GetAsync($"https://run.mocky.io/v3/7fe4c4e2-06f7-4803-843b-5e2297308043");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                //var json = File.ReadAllText(_countriesFilePath);
                var countries = JArray.Parse(json);

                foreach (var country in countries)
                {
                    var commonName = country["name"]?["common"]?.ToString();

                    if (string.Equals(commonName, countryName, StringComparison.OrdinalIgnoreCase))
                    {

                        var currency = country["currencies"]?.First;

                        if (currency == null)
                            return new CurrencyDTO
                            {
                                Name = "No se encontró la moneda para el país",
                                Symbol = "N/A"
                            };
                        else
                            return new CurrencyDTO
                            {
                                Name = currency?.First?["name"]?.ToString() ?? "Error",
                                Symbol = currency?.First?["symbol"]?.ToString() ?? "Error"
                            };
                    }
                }

                return new CurrencyDTO
                {
                    Name = "País no encontrado",
                    Symbol = "N/A"
                };
            }

            return new CurrencyDTO
            {
                Name = "Error al obtener la moneda",
                Symbol = "N/A"
            };
        }
    }
}

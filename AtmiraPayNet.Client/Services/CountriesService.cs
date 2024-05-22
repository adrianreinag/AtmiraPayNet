using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;


namespace AtmiraPayNet.Client.Services
{
    public class CountriesService(HttpClient http) : ICountriesService
    {

        private readonly HttpClient _http = http;

        public async Task<List<CountryDTO>> GetCountryList()
        {
            var json = await GetMockJson();

            if (json == null)
            {
                return [];
            }

            var countries = JsonConvert.DeserializeObject<List<CountryDTO>>(json) ?? [];

            var orderedCountries = countries.OrderBy(x => x.Name.Common).ToList();

            return orderedCountries;
        }

        public async Task<string?> GetCCA2ByCountryName(string countryName)
        {
            var json = await GetMockJson();

            if (json == null)
            {
                return null;
            }

            var countries = JsonConvert.DeserializeObject<List<CountryDTO>>(json) ?? [];

            foreach (var country in countries)
            {
                var commonName = country.Name.Common;

                if (string.Equals(commonName, countryName, StringComparison.OrdinalIgnoreCase))
                {
                    return country.CCA2;
                }
            }

            return null;
        }

        public async Task<string?> GetMockJson()
        {
            HttpResponseMessage response = await _http.GetAsync($"https://run.mocky.io/v3/7fe4c4e2-06f7-4803-843b-5e2297308043");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}


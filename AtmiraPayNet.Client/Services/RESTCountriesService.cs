using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AtmiraPayNet.Client.Services
{
    public class RESTCountriesService(HttpClient http) : IRESTCountriesService
    {
        private readonly HttpClient _http = http;

        public async Task<List<CountryDTO>> GetCountryList()
        {
            try
            {
                HttpResponseMessage response = await _http.GetAsync($"https://restcountries.com/v3.1/all?fields=name,cca2,currencies");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<List<CountryDTO>>(jsonResponse) ?? [];

                    jsonObject = [.. jsonObject.OrderBy(x => x.Name.Common)];

                    return jsonObject;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (HttpRequestException)
            {
                throw new Exception("Error connecting to the server");
            }
        }

        public async Task<string?> GetCCA2ByCountryName(string countryName)
        {
            HttpResponseMessage response = await _http.GetAsync($"https://restcountries.com/v3.1/name/{countryName}?fields=cca2");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();

                var cca2Response = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonResponse);

                if (cca2Response != null && cca2Response.Count > 0)
                {
                    return cca2Response[0]["cca2"];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

    }
}


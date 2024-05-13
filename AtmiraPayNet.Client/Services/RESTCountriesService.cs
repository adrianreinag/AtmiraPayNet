using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
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
                HttpResponseMessage response = await _http.GetAsync($"https://restcountries.com/v3.1/all?fields=name");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<CountryDTO>? countryList = JsonConvert.DeserializeObject<List<CountryDTO>>(jsonResponse) ?? [];

                    countryList.Sort(new CountryDTOComparer());

                    return countryList;
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
    }
}

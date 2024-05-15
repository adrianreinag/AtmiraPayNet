using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Newtonsoft.Json;

namespace AtmiraPayNet.Server.Services
{
    public class RESTCountriesService(HttpClient http) : IRESTCountriesService
    {
        private readonly HttpClient _http = http;

        public async Task<ResponseDTO<CurrencyDTO>> GetCurrencyByCountryName(string countryName)
        {
            try
            {
                HttpResponseMessage response = await _http.GetAsync($"https://restcountries.com/v3.1/name/{countryName}?fields=currencies");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var currencyResponse = JsonConvert.DeserializeObject<CurrencyResponseDTO[]>(jsonResponse);

                    if (currencyResponse != null && currencyResponse.Length > 0)
                    {
                        var currency = currencyResponse[0].Currencies.FirstOrDefault().Value;

                        return new ResponseDTO<CurrencyDTO>
                        {
                            StatusCode = StatusCodes.Status200OK,
                            Message = $"La moneda de {countryName} es {currency.Name} ({currency.Symbol})",
                            Value = currency
                        };
                    }
                    else
                    {
                        return new ResponseDTO<CurrencyDTO>
                        {
                            StatusCode = StatusCodes.Status204NoContent,
                            Message = "No se ha encontrado la moneda"
                        };
                    }
                }
                else
                {
                    return new ResponseDTO<CurrencyDTO>
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Error al obtener la moneda"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO<CurrencyDTO>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                };
            }
        }
    }
}

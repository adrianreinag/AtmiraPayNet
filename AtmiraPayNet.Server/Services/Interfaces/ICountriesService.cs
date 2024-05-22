using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Server.Services.Interfaces
{
    public interface ICountriesService
    {
        Task<CurrencyDTO> GetCurrencyByCountryName(string countryName);
    }
}

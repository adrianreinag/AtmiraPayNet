using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Server.Services.Interfaces
{
    public interface ICountriesService
    {
        CurrencyDTO GetCurrencyByCountryName(string countryName);
    }
}

using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Client.Services.Interfaces
{
    public interface ICountriesService
    {
        List<CountryDTO> GetCountryList();
        string? GetCCA2ByCountryName(string countryName);
    }
}

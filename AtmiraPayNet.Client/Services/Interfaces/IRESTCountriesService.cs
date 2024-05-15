using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Client.Services.Interfaces
{
    public interface IRESTCountriesService
    {
        Task<List<CountryDTO>> GetCountryList();
        Task<string?> GetCCA2ByCountryName(string countryName);
    }
}

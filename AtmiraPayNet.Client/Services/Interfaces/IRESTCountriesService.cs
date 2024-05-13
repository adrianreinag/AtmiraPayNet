using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Client.Services.Interfaces
{
    public interface IRESTCountriesService
    {
        Task<List<CountryDTO>> GetCountryList();
    }
}

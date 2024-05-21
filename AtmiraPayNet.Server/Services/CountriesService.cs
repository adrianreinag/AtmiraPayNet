﻿using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Newtonsoft.Json.Linq;

namespace AtmiraPayNet.Server.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly string _countriesFilePath = "Assets/Countries.json";

        public CurrencyDTO GetCurrencyByCountryName(string countryName)
        {
            var json = File.ReadAllText(_countriesFilePath);
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
    }
}

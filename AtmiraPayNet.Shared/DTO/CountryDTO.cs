namespace AtmiraPayNet.Shared.DTO
{
    public class CountryDTO
    {
        required public Name Name { get; set; }
        required public string CCA2 { get; set; }
        required public string CurrencyDTO { get; set; }
    }

    public class Name
    {
        required public string Common { get; set; }
        required public string Official { get; set; }
        required public Dictionary<string, NativeName> NativeName { get; set; }
    }

    public class NativeName
    {
        required public string Official { get; set; }
        required public string Common { get; set; }
    }

    public class CountryDTOComparer : IComparer<CountryDTO>
    {
        public int Compare(CountryDTO? x, CountryDTO? y) => string.Compare(x?.Name.Common, y?.Name.Common, StringComparison.OrdinalIgnoreCase);
    }
}

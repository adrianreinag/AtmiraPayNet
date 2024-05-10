namespace AtmiraPayNet.Client.Services.Interfaces
{
    public interface ILocalStorageService
    {
        Task Set(string key, string value);
        Task<string?> Get(string key);
    }
}

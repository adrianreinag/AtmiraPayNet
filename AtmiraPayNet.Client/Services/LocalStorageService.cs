using Microsoft.JSInterop;
using AtmiraPayNet.Client.Services.Interfaces;

namespace AtmiraPayNet.Client.Services
{
    public class LocalStorageService(IJSRuntime jsRuntime) : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        public async Task Set(string name, string value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", name, value);

        }

        public async Task<string?> Get(string name)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", name);
        }
    }

}

using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Client.Utility;
using AtmiraPayNet.Shared.DTO;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;

namespace AtmiraPayNet.Client.Services
{
    public class AuthService(HttpClient http, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider) : IAuthService
    {
        private readonly HttpClient _http = http;
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

        public async Task<bool> Register(RegisterDTO request)
        {
            var result = await _http.PostAsJsonAsync($"/api/auth/register", request);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<dynamic>(content);
                var token = response!.token;

                await _localStorageService.SetItemAsync("token", token);
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Login(LoginDTO request)
        {
            var result = await _http.PostAsJsonAsync($"/api/auth/login", request);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<Dictionary<string,string>>(content);

                string? token = response?["token"];

                if (token == null)
                {
                    return false;
                }

                await _localStorageService.SetItemAsync("token", token);
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("token");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}

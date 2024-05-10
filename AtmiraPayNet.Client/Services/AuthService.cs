using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AtmiraPayNet.Client.Services
{
    public class AuthService(HttpClient http) : IAuthService
    {
        private readonly HttpClient _http = http;

        public async Task<string?> Register(RegisterDTO request)
        {
            var result = await _http.PostAsJsonAsync($"/api/auth/register", request);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<dynamic>(content);

                return response!.token;
            }
            else
            {
                return null;
            }
        }

        public async Task<string?> Login(LoginDTO request)
        {
            var result = await _http.PostAsJsonAsync($"/api/auth/login", request);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<dynamic>(content);

                return response!.token;
            }
            else
            {
                return null;
            }
        }
    }
}

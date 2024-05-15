using AtmiraPayNet.Client.Services.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace AtmiraPayNet.Client.Utility
{
    public class CustomAuthenticationStateProvider(HttpClient http, ILocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly HttpClient _http = http;
        private readonly ILocalStorageService _localStorageService = localStorageService;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "apiauth");
            var authenticatedUser = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public async void MarkUserAsLoggedOut()
        {

            await _localStorageService.RemoveItemAsync("token");
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private static List<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();

            var jwtParts = jwt.Split('.');

            if (jwtParts.Length < 2)
            {
                return claims;
            }

            var payload = jwtParts[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs!.TryGetValue(ClaimTypes.Role, out object? roles);

            //if (roles != null)
            //{
            //    if (roles.ToString()!.Trim().StartsWith('['))
            //    {
            //        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

            //        foreach (var parsedRole in parsedRoles!)
            //        {
            //            claims.Add(new Claim(ClaimTypes.Role, parsedRole));
            //        }
            //    }
            //    else
            //    {
            //        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
            //    }

            //    keyValuePairs.Remove(ClaimTypes.Role);
            //}

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}


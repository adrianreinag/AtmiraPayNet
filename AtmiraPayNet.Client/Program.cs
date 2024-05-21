using AtmiraPayNet.Client;
using AtmiraPayNet.Client.Services;
using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Client.Utility;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7035") });
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSweetAlert2();


await builder.Build().RunAsync();

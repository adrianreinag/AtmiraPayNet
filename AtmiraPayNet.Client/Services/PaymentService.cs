using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace AtmiraPayNet.Client.Services
{
    public class PaymentService(HttpClient http, ILocalStorageService localStorageService, IJSRuntime jsRuntime) : IPaymentService
    {
        private readonly HttpClient _http = http;
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IJSRuntime _jsRuntime = jsRuntime;


        public async Task<bool> CreatePayment(PaymentDTO request)
        {
            var result = await _http.PostAsJsonAsync("api/payment", request);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdatePayment(Guid id, PaymentDTO request)
        {
            var result = await _http.PutAsJsonAsync($"api/payment?id={id}", request);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<SimplePaymentDTO>> GetPaymentList()
        {
            var token = await _localStorageService.GetItemAsync<string>("token") ?? throw new Exception("Token not found");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync("api/payment/all");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var paymentList = JsonConvert.DeserializeObject<List<SimplePaymentDTO>>(content);

                return paymentList ?? [];
            }
        }

        public async Task<PaymentDTO> GetPaymentById(Guid id)
        {
            var token = await _localStorageService.GetItemAsync<string>("token") ?? throw new Exception("Token not found");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync($"api/payment?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var payment = JsonConvert.DeserializeObject<PaymentDTO>(content);

                return payment!;
            }
        }

        public async Task DownloadPDF(Guid id)
        {
            var token = await _localStorageService.GetItemAsync<string>("token") ?? throw new Exception("Token not found");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync($"api/payment/pdf?id={id}");

            if (response.IsSuccessStatusCode)
            {
                string pdfBase64 = await response.Content.ReadAsStringAsync();

                await _jsRuntime.InvokeVoidAsync("downloadPdf", pdfBase64, $"{id}.pdf");
            }
        }
    }
}

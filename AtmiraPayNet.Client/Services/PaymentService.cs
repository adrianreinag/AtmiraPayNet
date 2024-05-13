using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AtmiraPayNet.Client.Services
{
    public class PaymentService(HttpClient http, ILocalStorageService localStorageService) : IPaymentService
    {
        private readonly HttpClient _http = http;
        private readonly ILocalStorageService _localStorageService = localStorageService;

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

        public async Task<List<SimplePaymentDTO>> GetListPayment()
        {
            var token = await _localStorageService.Get("token") ?? throw new Exception("Token not found");

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
            var token = await _localStorageService.Get("token") ?? throw new Exception("Token not found");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync($"api/payment?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var payment= JsonConvert.DeserializeObject<PaymentDTO>(content);

                return payment!;
            }
        }
    }
}

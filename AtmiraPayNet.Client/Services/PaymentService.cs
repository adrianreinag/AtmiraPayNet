using AtmiraPayNet.Client.Services.Interfaces;
using AtmiraPayNet.Shared;
using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Client.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> CreatePayment(PaymentDTO request)
        {
            throw new NotImplementedException();
        }

        public Task<List<SimplePaymentDTO>> GetListPayment()
        {
            throw new NotImplementedException();
        }

        public Task<PaymentDTO> GetPayment(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePayment(PaymentDTO request)
        {
            throw new NotImplementedException();
        }
    }
}

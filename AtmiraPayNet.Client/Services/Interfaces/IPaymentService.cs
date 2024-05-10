using AtmiraPayNet.Shared;
using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Client.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> CreatePayment(PaymentDTO request);
        Task<bool> UpdatePayment(PaymentDTO request);
        Task<PaymentDTO> GetPayment(Guid id);
        Task<List<SimplePaymentDTO>> GetListPayment();
    }
}

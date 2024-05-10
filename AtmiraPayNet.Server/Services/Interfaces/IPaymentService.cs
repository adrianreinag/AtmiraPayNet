using AtmiraPayNet.Server.Models;
using AtmiraPayNet.Shared;
using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Server.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ResponseDTO<Payment>> CreatePayment(PaymentDTO request);
        Task<ResponseDTO<Payment>> UpdatePayment(PaymentDTO request);
        Task<ResponseDTO<PaymentDTO>> GetPaymentById(Guid id);
        Task<ResponseDTO<List<SimplePaymentDTO>>> GetPaymentList();
    }
}

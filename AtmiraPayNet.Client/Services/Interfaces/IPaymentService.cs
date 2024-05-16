using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Client.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> CreatePayment(PaymentDTO request);
        Task<bool> UpdatePayment(Guid id, PaymentDTO request);
        Task<PaymentDTO> GetPaymentById(Guid id);
        Task<List<SimplePaymentDTO>> GetPaymentList();
        Task DownloadPDF(Guid id);
        Task<BankDTO> GetBankByIBAN(string iban);
    }
}

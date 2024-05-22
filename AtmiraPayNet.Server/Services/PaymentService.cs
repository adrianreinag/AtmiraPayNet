using AtmiraPayNet.Server.Context;
using AtmiraPayNet.Server.Models;
using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using AtmiraPayNet.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace AtmiraPayNet.Server.Services
{
    public class PaymentService(ApplicationDbContext context, ITokenService tokenService, IPDFService pdfService, ICountriesService countriesService) : IPaymentService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IPDFService _pdfService = pdfService;
        private readonly ICountriesService _countriesService = countriesService;

        public async Task<ResponseDTO<Payment>> CreatePayment(PaymentDTO request)
        {
            try
            {
                string? userId = _tokenService.GetCurrentUserId();

                if (userId == null)
                {
                    return new ResponseDTO<Payment>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "No autorizado"
                    };
                }

                Payment payment = new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId!,
                    SourceAccount = new BankAccount(request.SourceIBAN!, request.SourceBankName!, request.SourceBankCountry!),
                    DestinationAccount = new BankAccount(request.DestinationIBAN!, request.DestinationBankName!, request.DestinationBankCountry!),
                    IntermediaryAccount = request.IntermediaryIBAN != null ? new BankAccount(request.IntermediaryIBAN!, request.IntermediaryBankName!, request.IntermediaryBankCountry!) : null,
                    Amount = request.Amount,
                    Status = request.Status,
                    Address = new Address(request.Number!, request.Street!, request.City!, request.Country!, request.PostalCode!)
                };

                var createdPayment = _context.Payments.Add(payment).Entity;
                await _context.SaveChangesAsync();

                _context.Entry(createdPayment).Reference(p => p.User).Load();

                await CreatePaymentLetter(createdPayment);

                return new ResponseDTO<Payment>
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Transacción creada correctamente",
                    Value = createdPayment
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO<Payment>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }

        public async Task<ResponseDTO<Payment>> UpdatePayment(Guid id, PaymentDTO request)
        {
            try
            {
                string? userId = _tokenService.GetCurrentUserId();


                if (userId == null)
                {
                    return new ResponseDTO<Payment>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "No autorizado"
                    };
                }

                var payment = await _context.Payments.Where(p => p.Id == id).FirstOrDefaultAsync();

                if (payment == null)
                {
                    return new ResponseDTO<Payment>
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        Message = "No existe la transacción"
                    };
                }
                else if (payment.UserId != userId)
                {
                    return new ResponseDTO<Payment>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "No autorizado"
                    };
                }
                else if (payment.Status == Status.Generated)
                {
                    return new ResponseDTO<Payment>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Esta transacción ya está cerrada"
                    };
                }

                payment.SourceAccount = new BankAccount(request.SourceIBAN!, request.SourceBankName!, request.SourceBankCountry!);
                payment.DestinationAccount = new BankAccount(request.DestinationIBAN!, request.DestinationBankName!, request.DestinationBankCountry!);
                payment.IntermediaryAccount = request.IntermediaryIBAN != null ? new BankAccount(request.IntermediaryIBAN!, request.IntermediaryBankName!, request.IntermediaryBankCountry!) : null;
                payment.Amount = request.Amount;
                payment.Status = request.Status;
                payment.Address = new Address(request.Number!, request.Street!, request.City!, request.Country!, request.PostalCode!);

                var updatedPayment = _context.Payments.Update(payment).Entity;
                await _context.SaveChangesAsync();

                _context.Entry(updatedPayment).Reference(p => p.User).Load();

                await CreatePaymentLetter(updatedPayment);

                return new ResponseDTO<Payment>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Transacción actualizada correctamente",
                    Value = updatedPayment
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO<Payment>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }


        private async Task CreatePaymentLetter(Payment payment)
        {
            if (payment.Status == Status.Generated)
            {
                Dictionary<string, string> keyValuePairs = new()
                {
                    { "PAYMENT_ID", payment.Id.ToString() },
                    { "USER_ID", payment.UserId.ToString() },
                    { "USER_FULLNAME", payment.User!.FullName },
                    { "USER_USERNAME", payment.User.UserName! },
                    { "SOURCE_IBAN", payment.SourceAccount!.Iban },
                    { "SOURCE_BANK_NAME", payment.SourceAccount!.BankName },
                    { "DESTINATION_IBAN", payment.DestinationAccount!.Iban },
                    { "DESTINATION_BANK_NAME", payment.DestinationAccount!.BankName },
                    { "INTERMEDIARY_IBAN", payment.IntermediaryAccount?.Iban ?? "" },
                    { "INTERMEDIARY_BANK_NAME", payment.IntermediaryAccount?.BankName ?? "" },
                    { "AMOUNT", payment.Amount.ToString() },
                    { "ADDRESS", payment.Address.ToString()! }
                };

                string pdf = _pdfService.CreateBase64PDFFromTemplate("Assets/PaymentTemplate.html", keyValuePairs);

                PaymentLetter paymentLetter = new()
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    PDF = pdf
                };

                _context.PaymentLetters.Add(paymentLetter);

                await _context.SaveChangesAsync();

                payment.PaymentLetterId = paymentLetter.Id;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseDTO<PaymentDTO>> GetPaymentById(Guid id)
        {
            try
            {
                string? userId = _tokenService.GetCurrentUserId();

                if (userId == null)
                {
                    return new ResponseDTO<PaymentDTO>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "No autorizado"
                    };
                }

                var payment = await _context.Payments.Where(p => p.Id == id).FirstOrDefaultAsync();

                if (payment == null)
                {
                    return new ResponseDTO<PaymentDTO>
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        Message = "No existe la transacción"
                    };
                }
                else if (payment.UserId != userId)
                {
                    return new ResponseDTO<PaymentDTO>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "No autorizado"
                    };
                }

                PaymentDTO paymentDTO = new()
                {
                    Amount = payment.Amount,
                    SourceIBAN = payment.SourceAccount!.Iban,
                    SourceBankName = payment.SourceAccount.BankName,
                    SourceBankCountry = payment.SourceAccount.BankCountry,
                    PostalCode = payment.Address.PostalCode,
                    Street = payment.Address.Street,
                    City = payment.Address.City,
                    Country = payment.Address.Country,
                    Number = payment.Address.Number,
                    DestinationIBAN = payment.DestinationAccount!.Iban,
                    DestinationBankName = payment.DestinationAccount.BankName,
                    DestinationBankCountry = payment.DestinationAccount.BankCountry,
                    IntermediaryIBAN = payment.IntermediaryAccount?.Iban,
                    IntermediaryBankName = payment.IntermediaryAccount?.BankName,
                    IntermediaryBankCountry = payment.IntermediaryAccount?.BankCountry,
                    Status = payment.Status
                };

                return new ResponseDTO<PaymentDTO>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Transacción encontrada",
                    Value = paymentDTO
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO<PaymentDTO>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }

        public async Task<ResponseDTO<List<SimplePaymentDTO>>> GetPaymentList()
        {
            try
            {
                string? userId = _tokenService.GetCurrentUserId();

                if (userId == null)
                {
                    return new ResponseDTO<List<SimplePaymentDTO>>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "No autorizado"
                    };
                }

                var payments = await _context.Payments.Where(p => p.UserId == userId).ToListAsync();

                var paymentDTOs = new List<SimplePaymentDTO>();

                Dictionary<string, CurrencyDTO> currencies = [];

                foreach (var payment in payments)
                {

                    CurrencyDTO? currencyDTO = null;

                    if (!currencies.TryGetValue(payment.SourceAccount!.BankCountry, out CurrencyDTO? value))
                    {
                        currencyDTO = await _countriesService.GetCurrencyByCountryName(payment.SourceAccount!.BankCountry);

                        currencies.Add(payment.SourceAccount!.BankCountry, currencyDTO);
                    }
                    else
                    {
                        currencyDTO = value;
                    }


                    paymentDTOs.Add(new SimplePaymentDTO
                    {
                        Id = payment.Id,
                        SourceBankName = payment.SourceAccount!.BankName,
                        DestinationBankName = payment.DestinationAccount!.BankName,
                        Amount = payment.Amount,
                        Currency = currencyDTO,
                        Status = payment.Status
                    });
                }

                return new ResponseDTO<List<SimplePaymentDTO>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Transacciones encontradas",
                    Value = paymentDTOs
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO<List<SimplePaymentDTO>>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }

        public Task<ResponseDTO<string>> GetPaymentPDF(Guid id)
        {
            var payment = _context.Payments.Include(p => p.PaymentLetter).FirstOrDefault(p => p.Id == id) ?? throw new ArgumentException("El pago no existe");

            if (payment.PaymentLetter == null)
            {
                return Task.FromResult(new ResponseDTO<string>
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = "No se ha generado la carta de pago"
                });
            }

            return Task.FromResult(new ResponseDTO<string>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Carta de pago encontrada",
                Value = payment.PaymentLetter.PDF
            });
        }
    }
}

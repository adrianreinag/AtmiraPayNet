using AtmiraPayNet.Server.Context;
using AtmiraPayNet.Server.Models;
using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using AtmiraPayNet.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace AtmiraPayNet.Server.Services
{
    public class PaymentService(ApplicationDbContext context, ITokenService tokenService, IPDFService pdfService, IRESTCountriesService restCountriesService) : IPaymentService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IPDFService _pdfService = pdfService;
        private readonly IRESTCountriesService _restCountriesService = restCountriesService;

        public async Task<ResponseDTO<Payment>> CreatePayment(PaymentDTO request)
        {
            try
            {
                string? userId = _tokenService.GetCurrentUserId();

                var responseValidation = await ValidatePaymentDataAndUser(userId, request);

                if (responseValidation.StatusCode != StatusCodes.Status200OK)
                {
                    return new ResponseDTO<Payment>
                    {
                        StatusCode = responseValidation.StatusCode,
                        Message = responseValidation.Message
                    };
                }

                Payment payment = new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId!,
                    SourceAccountId = responseValidation.Value.Item1!.Id,
                    DestinationAccountId = responseValidation.Value.Item2!.Id,
                    IntermediaryAccountId = responseValidation.Value.Item3?.Id,
                    Amount = request.Amount,
                    Status = request.Status,
                    Address = new Address(request.Number!, request.Street!, request.City!, request.Country!, request.PostalCode!)
                };

                var createdPayment = _context.Payments.Add(payment).Entity;
                await _context.SaveChangesAsync();

                createdPayment = PaymentIncludes(createdPayment);

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

                var responseValidation = await ValidatePaymentDataAndUser(userId, request);

                if (responseValidation.StatusCode != StatusCodes.Status200OK)
                {
                    return new ResponseDTO<Payment>
                    {
                        StatusCode = responseValidation.StatusCode,
                        Message = responseValidation.Message
                    };
                }

                Guid? paymentId = id;

                if (paymentId == null)
                {
                    return new ResponseDTO<Payment>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "No se ha proporcionado el identificador de la transacción"
                    };
                }

                var payment = await _context.Payments.Where(p => p.Id == paymentId).FirstOrDefaultAsync();

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

                payment.SourceAccountId = responseValidation.Value.Item1!.Id;
                payment.DestinationAccountId = responseValidation.Value.Item2!.Id;
                payment.IntermediaryAccountId = responseValidation.Value.Item3?.Id;
                payment.Amount = request.Amount;
                payment.Status = request.Status;
                payment.Address = new Address(request.Number!, request.Street!, request.City!, request.Country!, request.PostalCode!);

                var updatedPayment = _context.Payments.Update(payment).Entity;
                await _context.SaveChangesAsync();

                updatedPayment = PaymentIncludes(updatedPayment);

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

        private async Task<ResponseDTO<(BankAccount?, BankAccount?, BankAccount?)>> ValidatePaymentDataAndUser(string? userId, PaymentDTO request)
        {
            if (userId == null)
            {
                return new ResponseDTO<(BankAccount?, BankAccount?, BankAccount?)>
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "No autorizado"
                };
            }

            BankAccount? sourceAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.IBAN == new IBAN(request.SourceIBAN!));

            if (sourceAccount == null)
            {
                return new ResponseDTO<(BankAccount?, BankAccount?, BankAccount?)>
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = "No existe la cuenta de origen",
                };
            }

            BankAccount? destinationAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.IBAN == new IBAN(request.DestinationIBAN!));

            if (destinationAccount == null)
            {
                return new ResponseDTO<(BankAccount?, BankAccount?, BankAccount?)>
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = "No existe la cuenta de destino",
                };
            }

            BankAccount? intermediaryAccount = null;

            if (request.IntermediaryIBAN != null)
            {
                intermediaryAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.IBAN == new IBAN(request.IntermediaryIBAN));

                if (intermediaryAccount == null && request.IntermediaryIBAN != null)
                {
                    return new ResponseDTO<(BankAccount?, BankAccount?, BankAccount?)>
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        Message = "No existe la cuenta intermediaria",
                    };
                }
            }

            return new ResponseDTO<(BankAccount?, BankAccount?, BankAccount?)>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Los datos de la transacción son correctos",
                Value = (sourceAccount, destinationAccount, intermediaryAccount)
            };
        }

        private Payment PaymentIncludes(Payment payment)
        {
            _context.Entry(payment)
                .Reference(p => p.SourceAccount)
                .Query()
                .Include(sa => sa.Bank)
                .Load();

            _context.Entry(payment)
                .Reference(p => p.DestinationAccount)
                .Query()
                .Include(da => da.Bank)
                .Load();

            if (payment.IntermediaryAccount != null)
            {
                _context.Entry(payment)
                    .Reference(p => p.IntermediaryAccount)
                    .Query()
                    .Include(ia => ia.Bank)
                    .Load();
            }

            _context.Entry(payment)
                .Reference(p => p.User)
                .Load();

            return payment;
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
                    { "SOURCE_IBAN", payment.SourceAccount!.IBAN.Value },
                    { "SOURCE_BANK_NAME", payment.SourceAccount.Bank!.Name },
                    { "DESTINATION_IBAN", payment.DestinationAccount!.IBAN.Value },
                    { "DESTINATION_BANK_NAME", payment.DestinationAccount.Bank!.Name },
                    { "INTERMEDIARY_IBAN", payment.IntermediaryAccount?.IBAN.Value ?? "" },
                    { "INTERMEDIARY_BANK_NAME", payment.IntermediaryAccount?.Bank!.Name ?? "" },
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

                var payment = await _context.Payments.Where(p => p.Id == id)
                                                     .Include(p => p.SourceAccount)
                                                     .Include(p => p.SourceAccount!.Bank)
                                                     .Include(p => p.DestinationAccount)
                                                     .Include(p => p.DestinationAccount!.Bank)
                                                     .Include(p => p.IntermediaryAccount)
                                                     .Include(p => p.IntermediaryAccount!.Bank)
                                                     .FirstOrDefaultAsync();

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
                    SourceIBAN = payment.SourceAccount!.IBAN.Value,
                    SourceBankName = payment.SourceAccount.Bank!.Name,
                    SourceBankCountry = payment.SourceAccount.Bank!.CountryName,
                    PostalCode = payment.Address.PostalCode,
                    Street = payment.Address.Street,
                    Number = payment.Address.Number,
                    DestinationIBAN = payment.DestinationAccount!.IBAN.Value,
                    DestinationBankName = payment.DestinationAccount.Bank!.Name,
                    DestinationBankCountry = payment.DestinationAccount.Bank.CountryName,
                    IntermediaryIBAN = payment.IntermediaryAccount?.IBAN.Value,
                    IntermediaryBankName = payment.IntermediaryAccount?.Bank?.Name,
                    IntermediaryBankCountry = payment.IntermediaryAccount?.Bank?.CountryName,
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

                var payments = await _context.Payments
                    .Where(p => p.UserId == userId)
                    .Include(p => p.SourceAccount)
                    .Include(p => p.SourceAccount!.Bank)
                    .Include(p => p.DestinationAccount)
                    .Include(p => p.DestinationAccount!.Bank)
                    .ToListAsync();

                var paymentDTOs = new List<SimplePaymentDTO>();

                Dictionary<string, CurrencyDTO> currencies = [];

                foreach (var payment in payments)
                {

                    CurrencyDTO? currencyDTO = null;

                    if (!currencies.TryGetValue(payment.SourceAccount!.Bank!.CountryName, out CurrencyDTO? value))
                    {
                        var responseCurrency = await _restCountriesService.GetCurrencyByCountryName(payment.SourceAccount!.Bank!.CountryName);

                        if (responseCurrency.StatusCode != StatusCodes.Status200OK)
                        {
                            return new ResponseDTO<List<SimplePaymentDTO>>
                            {
                                StatusCode = responseCurrency.StatusCode,
                                Message = responseCurrency.Message
                            };
                        }

                        currencyDTO = responseCurrency.Value!;

                        currencies.Add(payment.SourceAccount!.Bank!.CountryName, currencyDTO);
                    }
                    else
                    {
                        currencyDTO = value;
                    }


                    paymentDTOs.Add(new SimplePaymentDTO
                    {
                        Id = payment.Id,
                        SourceBankName = payment.SourceAccount!.Bank!.Name,
                        DestinationBankName = payment.DestinationAccount!.Bank!.Name,
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

        public Task<ResponseDTO<BankDTO>> GetBankByIBAN(string iban)
        {
            var bank = _context.BankAccounts.Include(b => b.Bank).FirstOrDefault(b => b.IBAN == new IBAN(iban))?.Bank;

            if (bank == null)
            {
                return Task.FromResult(new ResponseDTO<BankDTO>
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = "No se ha encontrado el banco"
                });
            }

            return Task.FromResult(new ResponseDTO<BankDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Banco encontrado",
                Value = new BankDTO
                {
                    Name = bank.Name,
                    CountryName = bank.CountryName
                }
            });
        }
    }
}

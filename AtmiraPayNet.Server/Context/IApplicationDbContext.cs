using AtmiraPayNet.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AtmiraPayNet.Server.Context
{
    public interface IApplicationDbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentLetter> PaymentLetters { get; set; }
    }
}

using AtmiraPayNet.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtmiraPayNet.Server.Context
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options), IApplicationDbContext
    {
        public required DbSet<Bank> Banks { get; set; }
        public required DbSet<BankAccount> BankAccounts { get; set; }
        public required DbSet<Payment> Payments { get; set; }
        public required DbSet<PaymentLetter> PaymentLetters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bank - BankAccount (1 - N)
            modelBuilder.Entity<BankAccount>()
                .HasOne(ba => ba.Bank)
                .WithMany(b => b.BankAccounts)
                .HasForeignKey(b => b.BankId)
                .OnDelete(DeleteBehavior.NoAction);

            // BankAccount (source) - Payment (1 - N)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.SourceAccount)
                .WithMany(ba => ba.SourcePayments)
                .HasForeignKey(p => p.SourceAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            // BankAccount (destination) - Payment (1 - N)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.DestinationAccount)
                .WithMany(ba => ba.DestinationPayments)
                .HasForeignKey(p => p.DestinationAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            // BankAccount (interediary) - Payment (1 - N)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.IntermediaryAccount)
                .WithMany(ba => ba.IntermediaryPayments)
                .HasForeignKey(p => p.IntermediaryAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            // User - Payment (1 - N)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Payment - PaymentLetter (1 - 1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentLetter)
                .WithOne(pl => pl.Payment)
                .HasForeignKey<PaymentLetter>(r => r.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Convert string to IBAN
            modelBuilder.Entity<BankAccount>()
                .Property(ba => ba.IBAN)
                .HasConversion(
                    v => v.Value,
                    v => new IBAN(v)
                );

            // JSON Column Address
            modelBuilder.Entity<Payment>()
                .OwnsOne(p => p.Address, a => a.ToJson());

            base.OnModelCreating(modelBuilder);
        }
    }
}

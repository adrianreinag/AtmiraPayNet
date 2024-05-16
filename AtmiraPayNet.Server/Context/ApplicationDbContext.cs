using AtmiraPayNet.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtmiraPayNet.Server.Context
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options), IApplicationDbContext
    {
        public required DbSet<Payment> Payments { get; set; }
        public required DbSet<PaymentLetter> PaymentLetters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            // JSON Columns
            modelBuilder.Entity<Payment>()
                .OwnsOne(p => p.Address, a => a.ToJson());

            modelBuilder.Entity<Payment>()
                .OwnsOne(p => p.SourceAccount, a => a.ToJson());

            modelBuilder.Entity<Payment>()
                .OwnsOne(p => p.DestinationAccount, a => a.ToJson());

            modelBuilder.Entity<Payment>()
                .OwnsOne(p => p.IntermediaryAccount, a => a.ToJson());

            base.OnModelCreating(modelBuilder);
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Explorer.Payment.Infrastructure.Database;

public class PaymentContext : DbContext
{
    public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("payment");
    }
}

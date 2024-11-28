using Explorer.Payment.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payment.Infrastructure.Database;

public class PaymentContext : DbContext
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<TourPurchaseToken> TourPurchaseTokens { get; set; }
    public DbSet<TourSale> TourSales {  get; set; }
    public DbSet<TourSaleTour> TourSaleTours { get; set; }
    public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("payment");

        modelBuilder.Entity<ShoppingCart>()
        .HasMany(s => s.Items)
        .WithOne(i => i.ShoppingCart)
        .HasForeignKey(i => i.ShoppingCartId);

        modelBuilder.Entity<TourSaleTour>()
            .HasKey(tst => new { tst.TourSaleId, tst.TourId });

        modelBuilder.Entity<TourSale>()
            .HasMany(ts => ts.Tours)
            .WithOne(tst => tst.TourSale)
            .HasForeignKey(tst => tst.TourSaleId);


        modelBuilder.Entity<TourSaleTour>()
            .HasOne(tst => tst.TourSale)
            .WithMany(ts => ts.Tours)
            .HasForeignKey(tst => tst.TourSaleId);
    }
}

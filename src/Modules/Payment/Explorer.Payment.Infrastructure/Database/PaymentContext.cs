using Explorer.Payment.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payment.Infrastructure.Database;

public class PaymentContext : DbContext
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<TourPurchaseToken> TourPurchaseTokens { get; set; }
    public DbSet<TourBundle> TourBundles { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<AdventureCoinNotification> AdventureCoinNotifications { get; set; }
    public DbSet<TourSale> TourSales {  get; set; }
    public DbSet<TourSaleTour> TourSaleTours { get; set; }
    public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("payment");

        // Configure ShoppingCart and its relationship with OrderItem
        modelBuilder.Entity<ShoppingCart>()
            .HasMany(s => s.Items)
            .WithOne(i => i.ShoppingCart)
            .HasForeignKey(i => i.ShoppingCartId);

        // Configure TPH mapping for OrderItem and its derived types
        modelBuilder.Entity<OrderItem>()
            .ToTable("OrderItems")                          // Shared table for TPH
            .HasDiscriminator<string>("Discriminator")      // Discriminator column
            .HasValue<OrderItem>("OrderItem")               // Base type
            .HasValue<TourOrderItem>("TourOrderItem")       // Derived type
            .HasValue<BundleOrderItem>("BundleOrderItem");  // Derived type

        modelBuilder.Entity<TourOrderItem>()
            .Property(t => t.TourId)
            .IsRequired();

        modelBuilder.Entity<BundleOrderItem>()
            .Property(b => b.BundleId)
            .IsRequired();

        modelBuilder.Entity<TourBundle>()
            .Property(te => te.Tours)
            .HasColumnType("jsonb");

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

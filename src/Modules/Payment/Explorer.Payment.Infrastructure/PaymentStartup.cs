using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payment.API.Internal;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using Explorer.Payment.Core.Mappers;
using Explorer.Payment.Core.UseCases.Author;
using Explorer.Payment.Core.UseCases.Tourist;
using Explorer.Payment.Infrastructure.Database;
using Explorer.Payment.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Payment.Infrastructure;

public static class PaymentStartup
{
    public static IServiceCollection ConfigurePaymentModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PaymentProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }
    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<ITourPurchaseTokenService_Internal, TourPurchaseTokenService>();
        services.AddScoped<IBundleService, BundleService>();
        services.AddScoped<ICouponAuthorService, CouponAuthorService>();
        services.AddScoped<ICouponTouristService, CouponTouristService>();
        services.AddScoped<IWalletService_Internal, WalletService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IAdventureCoinNotificationService, AdventureCoinNotificationService>();
        services.AddScoped<ITourSaleService, TourSaleService>();
        services.AddScoped<ITourSouvenirService, TourSouvenirService>();
        services.AddScoped<ITouristBonusService, TouristBonusService>();
    }
    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

        services.AddScoped(typeof(IOrderItemRepository), typeof(OrderItemRepository));
        services.AddScoped(typeof(ICrudRepository<PurchaseToken>), typeof(CrudDatabaseRepository<PurchaseToken, PaymentContext>));
        services.AddScoped(typeof(ICrudRepository<TourBundle>), typeof(CrudDatabaseRepository<TourBundle, PaymentContext>));
        services.AddScoped(typeof(ICrudRepository<Coupon>), typeof(CrudDatabaseRepository<Coupon, PaymentContext>));
        services.AddScoped(typeof(ICrudRepository<Wallet>), typeof(CrudDatabaseRepository<Wallet, PaymentContext>));
        services.AddScoped(typeof(IAdventureCoinNotificationRepository), typeof(AdventureCoinNotificationRepository));
        services.AddScoped(typeof(ICrudRepository<TourSale>), typeof(CrudDatabaseRepository<TourSale, PaymentContext>));
        services.AddScoped(typeof(ICrudRepository<OrderItem>), typeof(CrudDatabaseRepository<OrderItem, PaymentContext>));
        services.AddScoped(typeof(ICrudRepository<TourSouvenir>), typeof(CrudDatabaseRepository<TourSouvenir, PaymentContext>));
        services.AddScoped(typeof(ICrudRepository<TouristBonus>), typeof(CrudDatabaseRepository<TouristBonus, PaymentContext>));

        services.AddScoped<ITourSaleRepository, TourSaleRepository>();
        services.AddDbContext<PaymentContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("payment"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "payment")));
    }
}

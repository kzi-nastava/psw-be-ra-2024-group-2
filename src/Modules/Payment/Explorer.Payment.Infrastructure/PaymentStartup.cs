﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payment.API.Internal;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using Explorer.Payment.Core.Mappers;
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
    }
    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

        services.AddScoped(typeof(IOrderItemRepository), typeof(OrderItemRepository));
        services.AddScoped(typeof(ICrudRepository<TourPurchaseToken>), typeof(CrudDatabaseRepository<TourPurchaseToken, PaymentContext>));
        services.AddScoped(typeof(ICrudRepository<TourBundle>), typeof(CrudDatabaseRepository<TourBundle, PaymentContext>));

        services.AddDbContext<PaymentContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("payment"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "payment")));
    }
}

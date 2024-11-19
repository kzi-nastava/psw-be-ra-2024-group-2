using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payment.Core.Mappers;
using Explorer.Payment.Infrastructure.Database;
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

    }
    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddDbContext<PaymentContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("payment"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "payment")));
    }
}

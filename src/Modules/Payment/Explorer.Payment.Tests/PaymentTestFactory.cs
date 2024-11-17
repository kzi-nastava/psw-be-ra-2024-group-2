using Explorer.BuildingBlocks.Tests;
using Explorer.Payment.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Payment.Tests;

public class PaymentTestFactory : BaseTestFactory<PaymentContext>
{
    protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PaymentContext>));
        services.Remove(descriptor!);
        services.AddDbContext<PaymentContext>(SetupTestContext());

        // TODO: Add other DbContexts here

        return services;
    }
}

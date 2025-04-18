using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Mappers;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Stakeholders.Infrastructure;

public static class StakeholdersStartup
{
    public static IServiceCollection ConfigureStakeholdersModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(StakeholderProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }
    
    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IProfileService_Internal, PersonService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRatingApplicationService, RatingApplicationService>();
        services.AddScoped<ITokenGenerator, JwtGenerator>();
        services.AddScoped<IProfileMessageService, ProfileMessageService>();
        services.AddScoped<IProfileMessageNotificationService, ProfileMessageNotificationService>();
        services.AddScoped<IFAQService, FAQService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<Person>), typeof(CrudDatabaseRepository<Person, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<User>), typeof(CrudDatabaseRepository<User, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<Image>), typeof(CrudDatabaseRepository<Image, StakeholdersContext>));
        services.AddScoped(typeof(ITransactionRepository), typeof(TransactionRepository<StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<RatingApplication>), typeof(CrudDatabaseRepository<RatingApplication, StakeholdersContext>));
        services.AddScoped<IUserRepository, UserDatabaseRepository>();
        services.AddScoped<IImageRepository, ImageRepository<StakeholdersContext>>();
        services.AddScoped(typeof(ICrudRepository<ProfileMessage>), typeof(CrudDatabaseRepository<ProfileMessage, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<ProfileMessageNotification>), typeof(CrudDatabaseRepository<ProfileMessageNotification, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<FAQ>), typeof(CrudDatabaseRepository<FAQ, StakeholdersContext>));

        services.AddDbContext<StakeholdersContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("stakeholders"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "stakeholders")));
    }
}
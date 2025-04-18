using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Mappers;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Tours.Core.UseCases.Tourist;
using Explorer.Tours.Core.UseCases.Author;
using Explorer.Tours.Infrastructure.Database;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Tours.API.Internal.Administration;


namespace Explorer.Tours.Infrastructure;

public static class ToursStartup
{
    public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ToursProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }

    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<ITourReviewService, TourReviewService>();
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<ITourService_Internal, TourService>();
        services.AddScoped<IObjectService, ObjectService>();
        services.AddScoped<ITourIssueReportService, TourIssueReportService>();
        services.AddScoped<IClubInviteService, ClubInviteService>();
        services.AddScoped<IClubService, ClubService>();
        services.AddScoped<ICheckpointService, CheckpointService>();
        services.AddScoped<ITourPreferenceService, TourPreferenceService>();
        services.AddScoped<ITourIssueCommentService, TourIssueCommentService>();        
        services.AddScoped<ITourIssueNotificationService, TourIssueNotificationService>();
        services.AddScoped<ITourExecutionService, TourExecutionService>();
        services.AddScoped<IPersonalDairyService, PersonalDairyService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEventSubscriptionService, EventSubscriptionService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<Equipment>), typeof(CrudDatabaseRepository<Equipment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<Tour>), typeof(CrudDatabaseRepository<Tour, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<ClubInvite>), typeof(CrudDatabaseRepository<ClubInvite, ToursContext>));

        services.AddScoped<IClubInviteRepository, ClubInviteRepository>();
        services.AddScoped(typeof(ICrudRepository<Checkpoint>), typeof(CrudDatabaseRepository<Checkpoint, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourReview>), typeof(CrudDatabaseRepository<TourReview, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<Image>), typeof(CrudDatabaseRepository<Image, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourObject>), typeof(CrudDatabaseRepository<TourObject, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourIssueReport>), typeof(CrudDatabaseRepository<TourIssueReport, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourIssueComment>), typeof(CrudDatabaseRepository<TourIssueComment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourIssueNotification>), typeof(CrudDatabaseRepository<TourIssueNotification, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourPreference>), typeof(CrudDatabaseRepository<TourPreference, ToursContext>));

        services.AddScoped(typeof(ICrudRepository<Club>), typeof(CrudDatabaseRepository<Club, ToursContext>));

        services.AddScoped(typeof(ITransactionRepository), typeof(TransactionRepository<ToursContext>));

        //services.AddScoped(typeof(ITourDurationByTransportRepository), typeof(TourDurationByTransportRepository));

        services.AddScoped(typeof(ICrudRepository<TourExecution>), typeof(CrudDatabaseRepository<TourExecution, ToursContext>));

        //services.AddScoped(typeof(ICrudRepository<TourExecutionCheckpoint>), typeof(CrudDatabaseRepository<TourExecutionCheckpoint, ToursContext>));
        services.AddScoped(typeof(ITourExecutionRepository), typeof(TourExecutionRepository<ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TouristEquipment>), typeof(CrudDatabaseRepository<TouristEquipment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<PersonalDairy>), typeof(CrudDatabaseRepository<PersonalDairy, ToursContext>));
        services.AddScoped(typeof(IPersonalDairyRepository), typeof(PersonalDairyRepository<ToursContext>));
        services.AddScoped(typeof(ICrudRepository<EventSubscription>), typeof(CrudDatabaseRepository<EventSubscription, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<Event>), typeof(CrudDatabaseRepository<Event, ToursContext>));

        services.AddDbContext<ToursContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("tours"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
    }
}

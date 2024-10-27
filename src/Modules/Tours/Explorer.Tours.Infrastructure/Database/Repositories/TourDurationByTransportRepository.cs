using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TourDurationByTransportRepository : CrudDatabaseRepository<TourDurationByTransport, ToursContext>, ITourDurationByTransportRepository
{
    public TourDurationByTransportRepository(ToursContext dbContext) : base(dbContext)
    {
    }
}

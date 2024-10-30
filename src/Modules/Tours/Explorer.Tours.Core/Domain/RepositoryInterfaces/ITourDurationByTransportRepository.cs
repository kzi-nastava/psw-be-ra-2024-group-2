namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITourDurationByTransportRepository
{
    public TourDurationByTransport Get(long id);
    public TourDurationByTransport Create(TourDurationByTransport tourDurationByTransport);
}
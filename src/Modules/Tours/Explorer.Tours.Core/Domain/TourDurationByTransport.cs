using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.Core.Domain;

public class TourDurationByTransport : Entity
{
    public TransportType TransportType { get; private set; }
    public double Duration { get; private set; }
    public long TourId { get; private set; }
    public Tour Tour { get; private set; }
    public TourDurationByTransport(long tourId, TransportType transportType, double duration)
    {
        TourId = tourId;
        TransportType = transportType;
        Duration = duration;
    }

    public TourDurationByTransport(long tourId, string transportType, double duration)
    {
        TourId = tourId;
        TransportType = Enum.Parse<TransportType>(transportType);
        Duration = duration;
    }
}

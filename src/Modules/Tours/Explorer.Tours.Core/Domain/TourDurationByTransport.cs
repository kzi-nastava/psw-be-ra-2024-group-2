using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.Core.Domain;

public class TourDurationByTransport : Entity
{
    public Tour Tour { get; private set; } = null!;
    public TransportType TransportType { get; private set; }
    public double Duration { get; private set; }

    public TourDurationByTransport(TransportType transportType, double duration)
    {
        TransportType = transportType;
        Duration = duration;
    }

    public TourDurationByTransport(string transportType, double duration)
    {
        TransportType = Enum.Parse<TransportType>(transportType);
        Duration = duration;
    }
}

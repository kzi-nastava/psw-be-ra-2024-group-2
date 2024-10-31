using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System.Text.Json.Serialization;

namespace Explorer.Tours.Core.Domain;

public class TourDurationByTransport : ValueObject
{
    public TransportType TransportType { get; private set; }
    public double Duration { get; private set; }

    [JsonConstructor]
    public TourDurationByTransport(TransportType transportType, double duration)
    {
        TransportType = transportType;
        Duration = duration;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TransportType;
        yield return Duration;
    }
}

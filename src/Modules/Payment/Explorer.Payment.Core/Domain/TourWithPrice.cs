using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System.Text.Json.Serialization;

namespace Explorer.Payment.Core.Domain;

public sealed class TourWithPrice : ValueObject
{
    public long TourId { get; private set; }
    public double Price { get; private set; }
    public TourStatus TourStatus { get; private set; }

    [JsonConstructor]
    public TourWithPrice(long tourId, double price, TourStatus tourStatus)
    {
        TourId = tourId;
        Price = price;
        TourStatus = tourStatus;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TourId;
        yield return Price;
        yield return TourStatus;
    }
}

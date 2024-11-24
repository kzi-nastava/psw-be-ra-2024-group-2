using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Payment.API.Dtos;

public sealed class BundleItemDto
{
    public long TourId { get; set; }
    public string? TourName { get; set; } = string.Empty;
    public double Price { get; set; }
    public TourStatus TourStatus { get; set; }
}

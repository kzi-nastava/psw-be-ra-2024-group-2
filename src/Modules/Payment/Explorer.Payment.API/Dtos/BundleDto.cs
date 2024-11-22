using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Payment.API.Dtos;

public sealed class BundleDto
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public List<BundleItemDto> Tours { get; set; } = new();

    // These are used for returning the BundleDto object.
    // They are not filled.
    public long? AuthorId { get; set; }
    public TourStatus? Status { get; set; }
}

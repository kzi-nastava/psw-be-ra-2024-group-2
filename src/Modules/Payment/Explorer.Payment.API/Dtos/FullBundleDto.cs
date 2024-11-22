using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Payment.API.Dtos;

public sealed class FullBundleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public List<BundleItemDto> Tours { get; set; } = new();
    public long? AuthorId { get; set; }
    public BundleStatus? Status { get; set; }
}

namespace Explorer.Payment.API.Dtos;

public sealed class BundleOrderItemDto
{
    public double Price { get; set; }
    public long BundleId { get; set; }
    public long UserId { get; set; }
    public List<int> TourIds { get; set; } = new();
}

namespace Explorer.Payment.API.Dtos;

public class TourOrderItemBasicDto
{
    public long? TourId { get; set; }
    public long? BundleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public long UserId { get; set; }
    public double Price { get; set; }
    public DateTime TimeOfPurchase { get; set; }
    public bool Token { get; set; }
}

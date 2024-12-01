namespace Explorer.Payment.API.Dtos;

public sealed class TourOrderItemWithNameDto
{
    public double Price { get; set; }
    public long TourId { get; set; }
    public long UserId { get; set; }
    public string TourName { get; set; } = string.Empty;
}

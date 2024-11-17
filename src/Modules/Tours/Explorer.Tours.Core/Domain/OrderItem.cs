using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class OrderItem : Entity
{
    public string TourName { get; set; }
    public double Price { get; set; }
    public long TourId { get; set; }
    public long UserId { get; set; }
    public Boolean Token { get; set; }
    public long ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; }
    public OrderItem() { }
    public OrderItem(long tourId)
    {
        TourId = tourId;
    }
}

using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payment.Core.Domain;

public class TourOrderItem : OrderItem
{
    public long TourId { get; set; }
    public long AuthorId { get; set; }
    public TourOrderItem() { }

    public TourOrderItem(long tourId, long authorId, long userId, double price, DateTime timeOfPurchase, bool token, long shoppingCartId)
        : base(userId, price, timeOfPurchase, token, shoppingCartId)
    {
        TourId = tourId;
        AuthorId = authorId;
    }
}

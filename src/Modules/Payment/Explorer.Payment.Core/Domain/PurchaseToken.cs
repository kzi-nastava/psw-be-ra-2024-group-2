using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payment.Core.Domain;

public class PurchaseToken : Entity
{
    public long UserId { get; private set; }
    public double Price { get; private set; }
    public DateTime PurchaseTime { get; private set; }

    public PurchaseToken(long userId, double price, DateTime purchaseTime)
    {
        UserId = userId;
        Price = price;
        PurchaseTime = purchaseTime;
    }
}

namespace Explorer.Payment.Core.Domain;

public class TourPurchaseToken : PurchaseToken
{
    public long TourId { get; private set; }
    public TourPurchaseToken(long userId, long tourId, double price, DateTime purchaseTime) : base(userId, price, purchaseTime)
    {
        TourId = tourId;
    }
}

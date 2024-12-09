namespace Explorer.Payment.Core.Domain;

public class SouvenirPurchaseToken : PurchaseToken
{
    public long SouvenirId { get; private set; }
    public SouvenirPurchaseToken(long userId, long souvenirId, double price, DateTime purchaseTime) : base(userId, price, purchaseTime)
    {
        SouvenirId = souvenirId;
    }
}
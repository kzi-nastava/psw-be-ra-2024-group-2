using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payment.Core.Domain;

public sealed class BundleOrderItem : OrderItem
{
    public long BundleId { get; set; }
    public List<int> TourIds { get; set; } = new List<int>();

    public BundleOrderItem() { }

    public BundleOrderItem(long bundleId, long userId, double price, DateTime timeOfPurchase, bool token, long shoppingCartId)
        : base(userId, price, timeOfPurchase, token, shoppingCartId)
    {
        BundleId = bundleId;
    }
}

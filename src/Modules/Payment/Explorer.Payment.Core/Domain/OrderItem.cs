using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payment.Core.Domain;

public class OrderItem : Entity
{
    public long UserId { get; set; }
    public double Price { get; set; }
    public DateTime TimeOfPurchase { get; set; }
    public bool Token { get; set; }

    public long ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = default!;

    public OrderItem() { }

    public OrderItem(long userId, double price, DateTime timeOfPurchase, bool token, long shoppingCartId)
    {
        UserId = userId;
        Price = price;
        TimeOfPurchase = timeOfPurchase;
        Token = token;
        ShoppingCartId = shoppingCartId;
    }
}

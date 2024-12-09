using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain;

public sealed class SouvenirOrderItem : OrderItem
{
    public long SouvenirId { get; set; }
    public SouvenirOrderItem() {}
    public SouvenirOrderItem(long souvenirId, long userId, double price, DateTime timeOfPurchase, bool token, long shoppingCartId)
        : base(userId, price, timeOfPurchase, token, shoppingCartId)
    {
        SouvenirId = souvenirId;
    }
}

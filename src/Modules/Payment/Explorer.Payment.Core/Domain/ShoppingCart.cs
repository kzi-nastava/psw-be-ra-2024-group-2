using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain;

public class ShoppingCart : Entity
{
    public double TotalPrice { get; private set; } = 0.0;
    public List<OrderItem> Items { get; private set; }
    public long TouristId { get; private set; }

    public ShoppingCart(long touristId)
    {
        TouristId = touristId;
        Items = new List<OrderItem>();
    }

    public void AddItem(OrderItem item)
    {

        Items.Add(item);
        CalculateTotalPrice();
    }

    public void RemoveItem(OrderItem item)
    {

        Items.Remove(item);
        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = Items.Sum(item => item.Price);
    }

    public double GetTotalPrice()
    {
        return TotalPrice;
    }

    public void Checkout()
    {
        foreach (var item in Items)
        {
            item.Token = true;
        }
    }
}

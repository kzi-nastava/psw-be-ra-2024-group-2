using Explorer.Payment.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Public.Tourist;

public interface IShoppingCartService
{
    Result AddItemToCart(long userId, OrderItemDto orderItem);
    Result RemoveItemFromCart(long userId, OrderItemDto orderItem);
    Result Checkout(long userId);
    double GetTotalPrice(long userId);
    IEnumerable<OrderItemDto> GetOrderItems(long userId);
    IEnumerable<TourPaymentDto> GetPurchasedTours(long userId);
}

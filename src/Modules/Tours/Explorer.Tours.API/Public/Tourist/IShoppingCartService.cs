using System.Collections.Generic;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Public.Tourist.DTOs;
using FluentResults;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface IShoppingCartService
    {
        Result AddItemToCart(long userId, OrderItemDto orderItem);
        Result RemoveItemFromCart(long userId, OrderItemDto orderItem);
        Result Checkout(long userId); 
        double GetTotalPrice(long userId);
        IEnumerable<OrderItemDto> GetOrderItems(long userId);

    }
}

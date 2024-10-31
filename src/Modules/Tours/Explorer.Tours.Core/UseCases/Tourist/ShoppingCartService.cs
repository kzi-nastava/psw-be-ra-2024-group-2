using AutoMapper;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.API.Public.Tourist.DTOs;
using Explorer.Tours.Core.Domain;
using System.Collections.Generic;
using FluentResults;
using Explorer.BuildingBlocks.Core.UseCases;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IMapper _mapper;

    public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IMapper mapper)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _mapper = mapper;
    }

    public Result AddItemToCart(long userId, OrderItemDto orderItemDto)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        var orderItem = _mapper.Map<OrderItem>(orderItemDto);
        cart.AddItem(orderItem);
        _shoppingCartRepository.Update(cart);
        return Result.Ok(); 
    }

    public Result RemoveItemFromCart(long userId, OrderItemDto orderItemDto)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        var orderItem = _mapper.Map<OrderItem>(orderItemDto);
        cart.RemoveItem(orderItem);
        _shoppingCartRepository.Update(cart);
        return Result.Ok(); 
    }

    public Result Checkout(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        cart.Checkout();
        return Result.Ok();
    }

    public double GetTotalPrice(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        return cart.GetTotalPrice();
    }

  
}

using AutoMapper;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.API.Public.Tourist.DTOs;
using Explorer.Tours.Core.Domain;
using System.Collections.Generic;
using FluentResults;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ICrudRepository<Tour> _tourRepository;
    private readonly ICrudRepository<OrderItem> _orderItemRepository;
    private readonly IMapper _mapper;

    public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, ICrudRepository<Tour> tourRepository, ICrudRepository<OrderItem> orderItemRepository, IMapper mapper)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _tourRepository = tourRepository;
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    public Result AddItemToCart(long userId, OrderItemDto orderItemDto)
    {
        var allTours = _tourRepository.GetPaged(1, int.MaxValue);
        var tour = allTours.Results.FirstOrDefault(t => t.Id == orderItemDto.TourId);
        if (tour == null)
        {
            return Result.Fail("Item not found.");
        }

        var orderItem = _mapper.Map<OrderItem>(orderItemDto);

        var cart = _shoppingCartRepository.GetByUserId(userId);
        if (cart == null)
        {
            cart = new ShoppingCart();
            _shoppingCartRepository.Create(cart);

        }
        
        orderItem.TourName = tour.Name;
        orderItem.Price = tour.Price;
        orderItem.UserId = userId;
        cart.AddItem(orderItem);
        _shoppingCartRepository.Update(cart);

        return Result.Ok();
    }


    public Result RemoveItemFromCart(long userId, OrderItemDto orderItemDto)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        var orderItem = _mapper.Map<OrderItem>(orderItemDto);
        //cart.RemoveItem(orderItem);
        //_shoppingCartRepository.Update(cart);#
        var allItems = _orderItemRepository.GetPaged(1, int.MaxValue);
        var order = allItems.Results.FirstOrDefault(o => o.TourId == orderItemDto.TourId && o.ShoppingCartId == cart.Id);
        _orderItemRepository.Delete(order.Id);
        return Result.Ok(); 
    }

    public Result Checkout(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        cart.Checkout();
        _shoppingCartRepository.Update(cart);
        return Result.Ok();
    }

    public double GetTotalPrice(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        return cart.GetTotalPrice();
    }

  
}

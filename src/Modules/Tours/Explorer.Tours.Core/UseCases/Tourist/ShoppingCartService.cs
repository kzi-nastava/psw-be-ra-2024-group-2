using AutoMapper;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.API.Public.Tourist.DTOs;
using Explorer.Tours.Core.Domain;
using System.Collections.Generic;
using FluentResults;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

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

    /*public Result AddItemToCart(long userId, OrderItemDto orderItemDto)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        if (cart == null)
        {
            cart = new ShoppingCart(userId, orderItemDto.TourId);
            _shoppingCartRepository.Create(cart);
        }
        var orderItem = _mapper.Map<OrderItem>(orderItemDto);
        cart.AddItem(orderItem);
        _shoppingCartRepository.Update(cart);
        return Result.Ok();
    }*/
    public Result AddItemToCart(long userId, OrderItemDto orderItemDto)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        if (cart == null)
        {
            cart = new ShoppingCart(userId, orderItemDto.TourId);
            _shoppingCartRepository.Create(cart);
        }

        var allTours = _tourRepository.GetPaged(1, int.MaxValue);
        var tour = allTours.Results.FirstOrDefault(t => t.Id == orderItemDto.TourId);
        if (tour == null)
        {
            return Result.Fail("Item not found.");
        }

        var orderItem = _mapper.Map<OrderItem>(orderItemDto);
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
        var orderItemDeletion = cart.Items.FirstOrDefault(item =>
        item.TourId == orderItemDto.TourId &&
        item.UserId == userId);
        cart.RemoveItem(orderItem);
        _shoppingCartRepository.Update(cart);
        //_orderItemRepository.Delete(orderItemDeletion.Id);
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

using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal.Administration;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.UseCases.Tourist;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ITourService_Internal _tourService;
    private readonly ICrudRepository<OrderItem> _orderItemRepository;
    private readonly ICrudRepository<TourPurchaseToken> _tourPurchaseTokenRepository;
    private readonly IMapper _mapper;

    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository,
        ICrudRepository<OrderItem> orderItemRepository,
        ICrudRepository<TourPurchaseToken> tourPurchaseTokenRepository,
        ITourService_Internal tourService,
        IMapper mapper)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _orderItemRepository = orderItemRepository;
        _tourPurchaseTokenRepository = tourPurchaseTokenRepository;
        _tourService = tourService;
        _mapper = mapper;
    }

    public Result AddItemToCart(long userId, OrderItemDto orderItemDto)
    {

        var tour = _tourService.GetPaged(1, int.MaxValue).Value.Results
                      .FirstOrDefault(t => t.Id == orderItemDto.TourId);
        
        if (tour == null)
        {
            return Result.Fail("Item not found.");
        }

        var orderItem = _mapper.Map<OrderItem>(orderItemDto);
        orderItem.Price = tour.Price;
        orderItem.TourName = tour.Name;
        orderItem.UserId = userId;

        var cart = _shoppingCartRepository.GetByUserId(userId);
        if (cart == null)
        {
            cart = new ShoppingCart(userId);
            _shoppingCartRepository.Create(cart);
            cart.AddItem(orderItem);
            _shoppingCartRepository.Update(cart);
        }
        else
        {
            cart.AddItem(orderItem);
            _shoppingCartRepository.Update(cart);
        }

        return Result.Ok();
    }

    public Result RemoveItemFromCart(long userId, OrderItemDto orderItemDto)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        var orderItem = _mapper.Map<OrderItem>(orderItemDto);

        var allItems = _orderItemRepository.GetPaged(1, int.MaxValue);
        var order = allItems.Results.FirstOrDefault(o => o.TourId == orderItemDto.TourId && o.ShoppingCartId == cart.Id);
        _orderItemRepository.Delete(order.Id);
        return Result.Ok();
    }

    public Result Checkout(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);


        foreach (var item in cart.Items)
        {
            var purchaseToken = new TourPurchaseToken(userId, item.TourId, cart.TotalPrice, DateTime.UtcNow );
            _tourPurchaseTokenRepository.Create(purchaseToken);
        }


        cart.Items.Clear();
        _shoppingCartRepository.Update(cart);

        return Result.Ok();
    }

    public double GetTotalPrice(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        return cart.GetTotalPrice();
    }

    public IEnumerable<OrderItemDto> GetOrderItems(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);

        var orderItems = _orderItemRepository.GetPaged(1, int.MaxValue).Results
            .Where(item => item.UserId == cart.TouristId)
            .Select(item => new OrderItemDto
            {
                TourId = item.TourId,
                TourName = item.TourName,
                Price = item.Price,
                UserId = item.UserId,
            })
            .ToList();

        return orderItems;
    }

    public IEnumerable<TourPaymentDto> GetPurchasedTours(long userId)
    {
        // Pronađi sve TourPurchaseTokene za zadati userId
        var tokens = _tourPurchaseTokenRepository.GetPaged(1, int.MaxValue).Results
            .Where(token => token.UserId == userId)
            .ToList();

        // Ekstraktuj sve tourId vrednosti iz tih tokena
        var tourIds = tokens.Select(token => token.TourId).Distinct();
        var tours = _tourService.GetPaged(1, int.MaxValue).Value.Results
                      .Where(tour => tourIds.Contains(tour.Id)).ToList();

        // Mapiraj ture na DTOs i vrati ih
        var tourDtos = _mapper.Map<IEnumerable<TourPaymentDto>>(tours);
        return tourDtos;
    }
}

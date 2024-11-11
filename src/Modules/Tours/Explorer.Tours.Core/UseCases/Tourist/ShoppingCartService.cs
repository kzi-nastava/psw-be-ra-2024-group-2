using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Public.Tourist.DTOs;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using FluentResults;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ICrudRepository<Tour> _tourRepository;
    private readonly ICrudRepository<OrderItem> _orderItemRepository;
    private readonly ICrudRepository<TourPurchaseToken> _tourPurchaseTokenRepository;
    private readonly IMapper _mapper;

    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository,
        ICrudRepository<Tour> tourRepository,
        ICrudRepository<OrderItem> orderItemRepository,
        ICrudRepository<TourPurchaseToken> tourPurchaseTokenRepository,
        IMapper mapper)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _tourRepository = tourRepository;
        _orderItemRepository = orderItemRepository;
        _tourPurchaseTokenRepository = tourPurchaseTokenRepository;
        _mapper = mapper;
    }

    public Result AddItemToCart(long userId, OrderItemDto orderItemDto)
    {
        var tour = _tourRepository.GetPaged(1, int.MaxValue).Results
                      .FirstOrDefault(t => t.Id == orderItemDto.TourId);
        if (tour == null)
        {
            return Result.Fail("Item not found.");
        }

        var orderItem = _mapper.Map<OrderItem>(orderItemDto);
        orderItem.Price = tour.Price;
        orderItem.TourName = tour.Name;

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

        cart.RemoveItem(orderItem);
        _shoppingCartRepository.Update(cart);

        return Result.Ok();
    }

    public Result Checkout(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);

        
        foreach (var item in cart.Items)
        {
            var purchaseToken = new TourPurchaseToken(userId, item.TourId);
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
}

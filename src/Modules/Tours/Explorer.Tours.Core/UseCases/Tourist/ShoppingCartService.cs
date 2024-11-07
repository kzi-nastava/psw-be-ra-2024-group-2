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
    private readonly IMapper _mapper;

    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository,
        ICrudRepository<Tour> tourRepository,
        ICrudRepository<OrderItem> orderItemRepository, 
        IMapper mapper)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _tourRepository = tourRepository;
        _orderItemRepository = orderItemRepository;
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


            var orderItems = _orderItemRepository.GetPaged(1, int.MaxValue).Results
                .Where(item => item.UserId == cart.TouristId)
                .ToList();


            foreach (var item in orderItems)
            {
                item.Token = true;
                _orderItemRepository.Update(item); 
            }

            return Result.Ok();
        }
    
/*
    public Result Checkout(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        cart.Checkout();
        _shoppingCartRepository.Update(cart);

        return Result.Ok();
    }
*/
    public double GetTotalPrice(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        return cart.GetTotalPrice();
    }
}

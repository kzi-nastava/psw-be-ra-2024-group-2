using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Internal;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal.Administration;
using FluentResults;

namespace Explorer.Payment.Core.UseCases.Tourist;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ITourService_Internal _tourService;
    private readonly ICrudRepository<TourPurchaseToken> _tourPurchaseTokenRepository;
    private readonly IMapper _mapper;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly ICrudRepository<TourBundle> _tourBundleRepository;
    private readonly ICrudRepository<Wallet> _walletRepository;
    private readonly ICouponTouristService _couponTouristService;


    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository,
        ICrudRepository<TourPurchaseToken> tourPurchaseTokenRepository,
        ITourService_Internal tourService,
        IMapper mapper,
        IOrderItemRepository orderItemRepository,
        ICrudRepository<TourBundle> tourBundleRepository,
        ICrudRepository<Wallet> walletRepository,
        ICouponTouristService couponTouristService)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _tourPurchaseTokenRepository = tourPurchaseTokenRepository;
        _tourService = tourService;
        _mapper = mapper;
        _orderItemRepository = orderItemRepository;
        _tourBundleRepository = tourBundleRepository;
        _walletRepository = walletRepository;
        _couponTouristService = couponTouristService;
    }

    public Result<TourOrderItemDto> AddTourToCart(long userId, long tourId)
    {
        var tour = _tourService.GetById(tourId);

        if (tour.IsFailed)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Bundle item not found.");
        }

        var orderItem = new TourOrderItem()
        {
            UserId = userId,
            Price = tour.Value.Price,
            TourId = tourId,
            TimeOfPurchase = DateTime.UtcNow,
            AuthorId = tour.Value.UserId,
        };

        var cart = _shoppingCartRepository.GetByUserId(userId);

        if (cart is null)
        {
            cart = new ShoppingCart(userId);
            cart.AddTourItem(orderItem);

            _shoppingCartRepository.Create(cart);
        }
        else
        {
            if (cart.Items.Any(i => i is TourOrderItem t && t.TourId == tourId))
                return Result.Fail(FailureCode.Conflict).WithError("Tour already in cart.");

            // Check if any of the tours in the bundle are already in the cart
            foreach (var item in cart.Items)
            {
                if (item is BundleOrderItem bundleOrderItem && bundleOrderItem.TourIds.Contains((int)tourId))
                    return Result.Fail(FailureCode.Conflict).WithError("Tour already in cart.");
            }

            cart.AddTourItem(orderItem);
            _shoppingCartRepository.Update(cart);
        }

        var orderItemDto = new TourOrderItemDto
        {
            TourId = tourId,
            Price = tour.Value.Price,
            UserId = userId,
            AuthorId = tour.Value.UserId,
        };

        return orderItemDto;
    }

    public Result<BundleOrderItemDto> AddBundleToCart(long userId, long bundleId)
    {
        var bundle = _tourBundleRepository.Get(bundleId);

        if (bundle is null)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Bundle item not found.");
        }

        var tourIds = bundle.Tours;
        var tours = _tourService.GetPaged(1, int.MaxValue).Value.Results
                      .Where(t => tourIds.Contains(t.Id)).ToList();

        if (tours.Count != tourIds.Count)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Bundle item not found.");
        }

        var orderItem = new BundleOrderItem
        {
            UserId = userId,
            TimeOfPurchase = DateTime.UtcNow,
            Price = bundle.Price,
            BundleId = bundleId,
            TourIds = tourIds,
        };

        var cart = _shoppingCartRepository.GetByUserId(userId);

        if (cart is null)
        {
            cart = new ShoppingCart(userId);
            cart.AddBundleItem(orderItem);

            _shoppingCartRepository.Create(cart);
        }
        else
        {
            if (cart.Items.Any(i => i is BundleOrderItem b && b.BundleId == bundleId))
                return Result.Fail(FailureCode.Conflict).WithError("Bundle already in cart.");

            // Check if any of the tours in the bundle are already in the cart
            foreach (var tourId in tourIds)
            {
                if (cart.Items.Any(i => i is TourOrderItem t && t.TourId == tourId))
                    return Result.Fail(FailureCode.Conflict).WithError("Tour already in cart.");
            }

            // Check if any of the tours exists in another bundle in the cart
            foreach (var item in cart.Items)
            {
                if (item is BundleOrderItem bundleOrderItem && bundleOrderItem.TourIds.Intersect(tourIds).Any())
                    return Result.Fail(FailureCode.Conflict).WithError("Tour already in cart.");
            }

            cart.AddBundleItem(orderItem);
            _shoppingCartRepository.Update(cart);
        }

        var orderItemDto = new BundleOrderItemDto
        {
            BundleId = bundleId,
            Price = bundle.Price,
            TourIds = tourIds,
            UserId = userId,
        };

        return orderItemDto;
    }

    public Result<TourOrderItemDto> RemoveTourItemFromCart(long userId, long tourId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);

        var allItems = _orderItemRepository.GetAllTours();

        var order = allItems.FirstOrDefault(o => o.TourId == tourId && o.ShoppingCartId == cart.Id);

        if (order is null)
            return Result.Fail(FailureCode.NotFound).WithError("Bundle item not found.");

        _orderItemRepository.Delete(order.Id);

        return new TourOrderItemDto
        {
            TourId = order.TourId,
            Price = order.Price,
            UserId = userId,
        };
    }

    public Result<BundleOrderItemDto> RemoveBundleItemFromCart(long userId, long bundleId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);

        var allItems = _orderItemRepository.GetAllBundles();

        var order = allItems.FirstOrDefault(o => o.BundleId == bundleId && o.ShoppingCartId == cart.Id);

        if (order is null)
            return Result.Fail(FailureCode.NotFound).WithError("Bundle item not found.");

        _orderItemRepository.Delete(order.Id);

        return new BundleOrderItemDto
        {
            BundleId = order.BundleId,
            Price = order.Price,
            TourIds = order.TourIds,
            UserId = userId,
        };
    }

    public Result Checkout(long userId, string couponCode)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        var wallet = _walletRepository.GetPaged(1, int.MaxValue).Results
                          .FirstOrDefault(w => w.UserId == userId);

        float totalPrice = 0;
        long tourIdForDiscount = -10;
        float discountPercentage = -10;

        // Apply coupon if provided
        if (couponCode != "empty")
        {
            var coupon = _couponTouristService.UseCoupon(couponCode).Value;
            discountPercentage = (float)coupon.DiscountPercentage;

            if (coupon != null)
            {
                var matchingTours = cart.Items
                    .OfType<TourOrderItem>()
                    .Where(item => item.AuthorId == coupon.AuthorId)
                    .ToList();

                if (matchingTours.Any())
                {
                    tourIdForDiscount = coupon.AllToursDiscount
                        ? matchingTours.OrderByDescending(t => t.Price).First().TourId
                        : matchingTours.First().TourId;
                }
            }
        }

        // Calculate total price with discount for applicable tours
        totalPrice = cart.Items.Sum(item =>
        {
            if (item is TourOrderItem tourOrderItem)
            {
                return tourOrderItem.TourId == tourIdForDiscount
                    ? (float)tourOrderItem.Price * ((100 - discountPercentage) / 100)
                    : (float)tourOrderItem.Price;
            }

            return item is BundleOrderItem bundleOrderItem
                ? (float)bundleOrderItem.Price
                : 0;
        });

        // Check if user has enough Adventure Coins
        if (wallet.AdventureCoinsBalance < totalPrice)
        {
            return Result.Fail("Not enough Adventure Coins to complete the purchase.");
        }

        // Create purchase tokens for each item in cart
        foreach (var item in cart.Items)
        {
            if (item is TourOrderItem tourOrderItem)
            {
                _tourPurchaseTokenRepository.Create(new TourPurchaseToken(userId, tourOrderItem.TourId, totalPrice, DateTime.UtcNow));
            }

            if (item is BundleOrderItem bundleOrderItem)
            {
                foreach (var tourId in bundleOrderItem.TourIds)
                {
                    _tourPurchaseTokenRepository.Create(new TourPurchaseToken(userId, tourId, totalPrice, DateTime.UtcNow));
                }
            }
        }

        // Deduct funds and update wallet and cart
        wallet.SubtractFunds(totalPrice);
        _walletRepository.Update(wallet);

        cart.Items.Clear();
        _shoppingCartRepository.Update(cart);

        return Result.Ok();
    }


    public double GetTotalPrice(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);
        return cart.GetTotalPrice();
    }

    public List<TourOrderItemBasicDto> GetOrderItems(long userId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);

        if (cart is null)
            return new List<TourOrderItemBasicDto>();

        var tourItems = new List<TourOrderItem>();

        foreach (var item in cart.Items)
        {
            if (item is TourOrderItem tourOrderItem)
                tourItems.Add(tourOrderItem);
        }

        var bundleItems = new List<BundleOrderItem>();

        foreach (var item in cart.Items)
        {
            if (item is BundleOrderItem bundleOrderItem)
                bundleItems.Add(bundleOrderItem);
        }

        var orderItems = new List<TourOrderItemBasicDto>();
        
        orderItems.AddRange(tourItems.Select(item => { 
            return new TourOrderItemBasicDto
            {
                TourId = item.TourId,
                Price = item.Price,
                TimeOfPurchase = item.TimeOfPurchase,
                Token = item.Token,
                UserId = item.UserId,
                Name = _tourService.GetById(item.TourId).Value.Name,
                AuthorId = item.AuthorId,
            };
        }));

        orderItems.AddRange(bundleItems.Select(item =>
        {
            return new TourOrderItemBasicDto
            {
                BundleId = item.BundleId,
                Price = item.Price,
                TimeOfPurchase = item.TimeOfPurchase,
                Token = item.Token,
                UserId = item.UserId,
                Name = _tourBundleRepository.Get(item.BundleId).Name,
            };
        }));

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

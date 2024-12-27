using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Internal;
using Explorer.Payment.API.Public.Author;
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
    private readonly ICrudRepository<PurchaseToken> _purchaseTokenRepository;
    private readonly IMapper _mapper;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly ICrudRepository<TourBundle> _tourBundleRepository;
    private readonly ICrudRepository<TourSouvenir> _tourSouvenirRepository;
    private readonly ICrudRepository<Wallet> _walletRepository;
    private readonly ICouponTouristService _couponTouristService;
    private readonly ITourSaleService _tourSaleService;

    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository,
        ICrudRepository<PurchaseToken> tourPurchaseTokenRepository,
        ITourService_Internal tourService,
        IMapper mapper,
        IOrderItemRepository orderItemRepository,
        ICrudRepository<TourBundle> tourBundleRepository,
        ICrudRepository<TourSouvenir> tourSouvenirRepository,
        ICrudRepository<Wallet> walletRepository,
        ITourSaleService tourSaleService,
        ICouponTouristService couponTouristService)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _purchaseTokenRepository = tourPurchaseTokenRepository;
        _tourService = tourService;
        _mapper = mapper;
        _orderItemRepository = orderItemRepository;
        _tourBundleRepository = tourBundleRepository;
        this._tourSouvenirRepository = tourSouvenirRepository;
        _walletRepository = walletRepository;
        _couponTouristService = couponTouristService;
        _tourSaleService = tourSaleService;
    }

    public Result<TourOrderItemDto> AddTourToCart(long userId, long tourId)
    {
        var tour = _tourService.GetById(tourId);

        var salesResult = _tourSaleService.GetAll();

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

            var sales = salesResult.Value;
            foreach (var sale in sales)
            {
                if (sale.Tours.Any(t => t.Id == tourId))
                {
                    var discountRate = sale.DiscountPercentage / 100.0;

                    var priceAsDouble = (double)orderItem.Price;
                    var discountedPriceDouble = priceAsDouble * (1.0 - discountRate);

                    orderItem.Price = discountedPriceDouble;

                    break;
                }
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

    public Result<SouvenirOrderItemDto> AddSouvenirToCart(long userId, long souvenirId)
    {
        var souvenir = _tourSouvenirRepository.Get(souvenirId);

        if (souvenir is null)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Souvenir item not found.");
        }

        var orderItem = new SouvenirOrderItem
        {
            UserId = userId,
            TimeOfPurchase = DateTime.UtcNow,
            Price = souvenir.Price,
            SouvenirId = souvenirId,
        };

        var cart = _shoppingCartRepository.GetByUserId(userId);

        if (cart is null)
        {
            cart = new ShoppingCart(userId);
            cart.AddSouvenirItem(orderItem);

            _shoppingCartRepository.Create(cart);
        }
        else
        {
            if (cart.Items.Any(i => i is SouvenirOrderItem s && s.SouvenirId == souvenirId))
                return Result.Fail(FailureCode.Conflict).WithError("Souvenir already in cart.");

            cart.AddSouvenirItem(orderItem);
            _shoppingCartRepository.Update(cart);
        }

        var orderItemDto = new SouvenirOrderItemDto
        {
            SouvenirId = souvenirId,
            Price = souvenir.Price,
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

    public Result<SouvenirOrderItemDto> RemoveSouvenirItemFromCart(long userId, long souvenirId)
    {
        var cart = _shoppingCartRepository.GetByUserId(userId);

        var allItems = _orderItemRepository.GetAllSouvenirs();

        var order = allItems.FirstOrDefault(o => o.SouvenirId == souvenirId && o.ShoppingCartId == cart.Id);

        if (order is null)
            return Result.Fail(FailureCode.NotFound).WithError("Souvenir item not found.");

        _orderItemRepository.Delete(order.Id);

        return new SouvenirOrderItemDto
        {
            SouvenirId = order.SouvenirId,
            Price = order.Price,
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

            if(item is BundleOrderItem bundleOrderitem)
            {
                return (float)bundleOrderitem.Price;
            }

            if (item is SouvenirOrderItem souvenirOrderItem)
            {
                return (float)souvenirOrderItem.Price;
            }

            return 0f;
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
                _purchaseTokenRepository.Create(new TourPurchaseToken(userId, tourOrderItem.TourId, tourOrderItem.Price, DateTime.UtcNow));
            }

            if (item is BundleOrderItem bundleOrderItem)
            {
                foreach (var tourId in bundleOrderItem.TourIds)
                {
                    _purchaseTokenRepository.Create(new TourPurchaseToken(userId, tourId, bundleOrderItem.Price, DateTime.UtcNow));
                }
            }

            if (item is SouvenirOrderItem souvenirOrderItem)
            {
                _purchaseTokenRepository.Create(new SouvenirPurchaseToken(userId, souvenirOrderItem.SouvenirId, souvenirOrderItem.Price, DateTime.UtcNow));

                var souvenir = _tourSouvenirRepository.Get(souvenirOrderItem.SouvenirId);

                souvenir.DecreaseCount(1);

                _tourSouvenirRepository.Update(souvenir);
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
        
        var souvenirItems = new List<SouvenirOrderItem>();

        foreach (var item in cart.Items)
        {
            if (item is SouvenirOrderItem souvenirOrderItem)
                souvenirItems.Add(souvenirOrderItem);
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

        orderItems.AddRange(souvenirItems.Select(item =>
        {
            return new TourOrderItemBasicDto
            {
                SouvenirId = item.SouvenirId,
                Price = item.Price,
                TimeOfPurchase = item.TimeOfPurchase,
                Token = item.Token,
                UserId = item.UserId,
                Name = _tourSouvenirRepository.Get(item.SouvenirId).Name,
            };
        }));

        return orderItems;
    }

    public IEnumerable<TourPaymentDto> GetPurchasedTours(long userId)
    {
        // Pronađi sve TourPurchaseTokene za zadati userId
        var tokens = _purchaseTokenRepository.GetPaged(1, int.MaxValue).Results
            .Where(token => token.UserId == userId)
            .ToList();

        // Ekstraktuj sve tourId vrednosti iz tih tokena
        var tourIds = tokens.Select(token => token is TourPurchaseToken tourToken ? tourToken.TourId : -1).ToList();
        var tours = _tourService.GetPaged(1, int.MaxValue).Value.Results
                      .Where(tour => tourIds.Contains(tour.Id)).ToList();

        // Mapiraj ture na DTOs i vrati ih
        var tourDtos = _mapper.Map<IEnumerable<TourPaymentDto>>(tours);
        return tourDtos;
    }

    public IEnumerable<TourSouvenirDto> GetPurchasedSouvenirs(long userId)
    {
        var tokens = _purchaseTokenRepository.GetPaged(1, int.MaxValue).Results
            .Where(token => token.UserId == userId)
            .ToList();

        var souvenirIds = tokens.Select(token => token is SouvenirPurchaseToken souvenirToken ? souvenirToken.SouvenirId : -1).ToList();

        var souvenirs = _tourSouvenirRepository.GetPaged(1, int.MaxValue).Results
                          .Where(souvenir => souvenirIds.Contains(souvenir.Id)).ToList();

        var souvenirDtos = _mapper.Map<IEnumerable<TourSouvenirDto>>(souvenirs);

        return souvenirDtos;
    }
}

using Explorer.Payment.API.Dtos;
using FluentResults;

namespace Explorer.Payment.API.Public.Tourist;

public interface IShoppingCartService
{
    Result<TourOrderItemDto> AddTourToCart(long userId, long tourId);
    Result<BundleOrderItemDto> AddBundleToCart(long userId, long bundleId);
    Result<SouvenirOrderItemDto> AddSouvenirToCart(long userId, long souvenirId);

    Result<TourOrderItemDto> RemoveTourItemFromCart(long userId, long tourId);
    Result<BundleOrderItemDto> RemoveBundleItemFromCart(long userId, long bundleId);
    Result<SouvenirOrderItemDto> RemoveSouvenirItemFromCart(long userId, long souvenirId);

    Result Checkout(long userId, string couponCode);
    double GetTotalPrice(long userId);

    List<TourOrderItemBasicDto> GetOrderItems(long userId);

    IEnumerable<TourPaymentDto> GetPurchasedTours(long userId);
    IEnumerable<TourSouvenirDto> GetPurchasedSouvenirs(long userId);
}

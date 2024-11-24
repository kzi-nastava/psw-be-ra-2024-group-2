using Explorer.Payment.API.Dtos;
using FluentResults;

namespace Explorer.Payment.API.Public.Tourist;

public interface IShoppingCartService
{
    Result<TourOrderItemDto> AddTourToCart(long userId, long tourId);
    Result<BundleOrderItemDto> AddBundleToCart(long userId, long bundleId);
    Result<TourOrderItemDto> RemoveTourItemFromCart(long userId, long tourId);
    Result<BundleOrderItemDto> RemoveBundleItemFromCart(long userId, long bundleId);
    Result Checkout(long userId);
    double GetTotalPrice(long userId);
    List<TourOrderItemBasicDto> GetOrderItems(long userId);
    IEnumerable<TourPaymentDto> GetPurchasedTours(long userId);
}

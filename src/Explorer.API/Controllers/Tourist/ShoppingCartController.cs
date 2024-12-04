using Explorer.Payment.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Payment.API.Dtos;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/shopping-cart")]
public class ShoppingCartController : BaseApiController
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    [HttpPost("add/{tourId:long}")]
    public ActionResult<TourOrderItemDto> AddItemToCart(long tourId)
    {
        var result = _shoppingCartService.AddTourToCart(User.PersonId(), tourId);
        return CreateResponse(result);
    }

    [HttpPost("add-bundle/{bundleId:long}")]
    public ActionResult<BundleOrderItemDto> AddBundleToCart(long bundleId)
    {
        var result = _shoppingCartService.AddBundleToCart(User.PersonId(), bundleId);
        return CreateResponse(result);
    }

    [HttpDelete("remove/{tourId:long}")]
    public ActionResult<TourOrderItemDto> RemoveItemFromCart(long tourId)
    {
        var result = _shoppingCartService.RemoveTourItemFromCart(User.PersonId(), tourId);
        return CreateResponse(result);
    }

    [HttpDelete("remove-bundle/{bundleId:long}")]
    public ActionResult<BundleOrderItemDto> RemoveBundleFromCart(long bundleId)
    {
        var result = _shoppingCartService.RemoveBundleItemFromCart(User.PersonId(), bundleId);
        return CreateResponse(result);
    }

    [HttpGet("checkout")]
    public ActionResult Checkout([FromQuery] string couponCode)
    {
        var result = _shoppingCartService.Checkout(User.PersonId(), couponCode);
        return CreateResponse(result);
    }

    [HttpGet("total")]
    public ActionResult<double> GetTotalPrice()
    {
        var result = _shoppingCartService.GetTotalPrice(User.PersonId());
        return Ok(result);
    }

    [HttpGet("items")]
    public ActionResult GetOrderItems()
    {
        var result = _shoppingCartService.GetOrderItems(User.PersonId());
        return Ok(result);
    }

    [HttpGet("purchasedTours")]
    public ActionResult<IEnumerable<TourPaymentDto>> GetPurchasedTours()
    {
        var userId = User.PersonId();
        var result = _shoppingCartService.GetPurchasedTours(userId);
        return Ok(result);
    }
}

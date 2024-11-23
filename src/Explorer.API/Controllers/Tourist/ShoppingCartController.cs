using Explorer.Payment.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Payment.API.Dtos;

namespace Explorer.API.Controllers.Tourist
{
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
        public ActionResult AddItemToCart(long tourId)
        {
            var orderItemDto = new OrderItemDto { TourId = tourId };
            var result = _shoppingCartService.AddItemToCart(User.PersonId(), orderItemDto);
            return CreateResponse(result);
        }

        [HttpDelete("remove/{tourId:long}")]
        public ActionResult RemoveItemFromCart(long tourId)
        {
            var orderItemDto = new OrderItemDto { TourId = tourId };
            var result = _shoppingCartService.RemoveItemFromCart(User.PersonId(), orderItemDto);
            return CreateResponse(result);
        }

        [HttpGet("checkout")]
        public ActionResult Checkout()
        {
            var result = _shoppingCartService.Checkout(User.PersonId());
            return CreateResponse(result);
        }

        [HttpGet("total")]
        public ActionResult<double> GetTotalPrice()
        {
            var result = _shoppingCartService.GetTotalPrice(User.PersonId());
            return Ok(result);
        }


        [HttpGet("items")]
        public ActionResult<IEnumerable<OrderItemDto>> GetOrderItems()
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
}

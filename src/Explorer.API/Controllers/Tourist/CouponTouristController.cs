using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/coupon")]
    public class CouponTouristController : BaseApiController
    {
        private readonly ICouponTouristService _couponService;

        public CouponTouristController(ICouponTouristService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost]
        public ActionResult<CouponDto> UseCoupon([FromBody] CouponDto coupon)
        {
            var theCoupon = _couponService.UseCoupon(coupon.AuthorId, coupon.TourId, coupon.Code);
            return CreateResponse(theCoupon);
        }
    }
}

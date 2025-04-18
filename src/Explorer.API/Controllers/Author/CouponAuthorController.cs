﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/author/coupon")]
public class CouponAuthorController :BaseApiController
{
    private readonly ICouponAuthorService _couponService;

    public CouponAuthorController(ICouponAuthorService couponService)
    {
        _couponService = couponService;
    }

    [HttpPost]
    public ActionResult<CouponDto> CreateCoupon([FromBody] CouponDto coupon)
    {
        var newCoupon = _couponService.CreateCoupon(User.UserId(), coupon.TourId, coupon.DiscountPercentage, coupon.AllToursDiscount);
        return CreateResponse(newCoupon);
    }

    [HttpGet]
    public ActionResult<PagedResult<CouponDto>> GetAuthorCoupons()
    {
        var result = _couponService.GetAuthorCoupons(User.UserId());
        return CreateResponse(result.ToResult());
    }
}

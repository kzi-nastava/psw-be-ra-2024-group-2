using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Tests.Integration.Shopping;

[Collection("Sequential")]
public class CouponTests : BasePaymentIntegrationTest
{
    public CouponTests(PaymentTestFactory factory) : base(factory){}


    [Fact]
    public void CreatesCoupon_successfully()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateAuthorController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var couponDto = new CouponDto
        {
            Code = "",
            DiscountPercentage = 20,
            TourId = -100,
            AuthorId = -21,
            AllToursDiscount = true
        };

        // Act
        var result = ((OkObjectResult)controller.CreateCoupon(couponDto).Result).Value as CouponDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedCoupon = dbContext.Coupons.FirstOrDefault(b => b.DiscountPercentage == 20);
        storedCoupon.ShouldNotBeNull();
        storedCoupon.DiscountPercentage.ShouldBe(20);
        storedCoupon.TourId.ShouldBe(-100);
        storedCoupon.AuthorId.ShouldBe(-21);
        storedCoupon.AllToursDiscount.ShouldBe(true);
    }

    [Fact]
    public void RetrievesAuthorCoupons_successfully()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateAuthorController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        // Act
        var result = ((ObjectResult)controller.GetAuthorCoupons().Result)?.Value as PagedResult<CouponDto>;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Results[0].DiscountPercentage.ShouldBe(20);
        result.Results[0].TourId.ShouldBe(-100);
        result.Results[0].AuthorId.ShouldBe(-21);
        result.Results[0].AllToursDiscount.ShouldBe(true);
    }

    [Fact]
    public void UsesCoupon_successfully()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateTouristController(scope, "-30");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        // Act
        var result = ((ObjectResult)controller.UseCoupon("VD23HUIJ").Result)?.Value as CouponDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Code.ShouldBe("VD23HUIJ");
        result.DiscountPercentage.ShouldBe(20);
        result.TourId.ShouldBe(-100);
        result.AuthorId.ShouldBe(-21);
        result.AllToursDiscount.ShouldBe(true);
    }

    private static CouponAuthorController CreateAuthorController(IServiceScope scope, string userId)
    {
        return new CouponAuthorController(scope.ServiceProvider.GetRequiredService<ICouponAuthorService>())
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                            new Claim("id", userId),
                            new Claim("personId", userId)
                    }))
                }
            }
        };
    }

    private static CouponTouristController CreateTouristController(IServiceScope scope, string userId)
    {
        return new CouponTouristController(scope.ServiceProvider.GetRequiredService<ICouponTouristService>())
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                            new Claim("id", userId),
                            new Claim("personId", userId)
                    }))
                }
            }
        };
    }
}

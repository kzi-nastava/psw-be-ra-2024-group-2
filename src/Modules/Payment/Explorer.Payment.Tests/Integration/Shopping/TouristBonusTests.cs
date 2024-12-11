using Explorer.API.Controllers.Tourist;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Tests.Integration.Shopping
{
    [Collection("Sequential")]
    public class TouristBonusTests : BasePaymentIntegrationTest
    {
        public TouristBonusTests(PaymentTestFactory factory) : base(factory) { }

        [Fact]
        public void CreateTouristBonus_ShouldReturnCreatedTouristBonus()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();
            long touristId = 12345;
            int discountPercentage = 10;

            // Act
            var result = ((ObjectResult)controller.CreateTouristBonus(touristId, discountPercentage).Result)?.Value as TouristBonusDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.TouristId.ShouldBe(touristId);
            result.IsUsed.ShouldBe(false);

            // Assert - Database
            var storedEntity = dbContext.TouristBonuses.FirstOrDefault(i => i.TouristId == touristId);
            storedEntity.ShouldNotBeNull();
            storedEntity.TouristId.ShouldBe(touristId);
            storedEntity.IsUsed.ShouldBe(false);
        }

        [Fact]
        public async Task GetTouristBonusById_ShouldReturnTouristBonus()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();
            long touristId = -1;

            // Act
            var result = ((ObjectResult)controller.GetById(touristId).Result)?.Value as TouristBonusDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.TouristId.ShouldBe(touristId);
            result.IsUsed.ShouldBe(false);

            // Assert - Database
            var storedEntity = dbContext.TouristBonuses.FirstOrDefault(i => i.TouristId == touristId);
            storedEntity.ShouldNotBeNull();
            storedEntity.TouristId.ShouldBe(touristId);
            storedEntity.IsUsed.ShouldBe(false);
        }

        [Fact]
        public async Task UseTouristBonus_ShouldMarkCouponAsUsed()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();
            long touristId = -1;
            string couponCode = "ABCD123";

            // Act
            var result = ((ObjectResult)controller.UseTouristBonus(touristId, couponCode).Result)?.Value as TouristBonusDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.TouristId.ShouldBe(touristId);
            result.CouponCode.ShouldBe(couponCode);
            result.IsUsed.ShouldBe(true);

            // Assert - Database
            var storedEntity = dbContext.TouristBonuses.FirstOrDefault(i => i.TouristId == touristId);
            storedEntity.ShouldNotBeNull();
            storedEntity.TouristId.ShouldBe(touristId);
            result.CouponCode.ShouldBe(couponCode);
            storedEntity.IsUsed.ShouldBe(true);
        }

        [Fact]
        public async Task UseTouristBonus_InvalidCouponCode_ShouldReturnError()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();
            long touristId = -1;
            string invalidCouponCode = "INVALIDCODE";

            // Act
            var result = ((ObjectResult)controller.UseTouristBonus(touristId, invalidCouponCode).Result)?.Value as TouristBonusDto;

            // Assert - Response
            result.ShouldBeNull();

            // Assert - Database
            var storedEntity = dbContext.TouristBonuses.FirstOrDefault(i => i.TouristId == touristId && i.CouponCode == invalidCouponCode);
            storedEntity.ShouldBeNull();
        }



        private static TouristBonusController CreateController(IServiceScope scope)
        {
            return new TouristBonusController(scope.ServiceProvider.GetRequiredService<ITouristBonusService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

using Explorer.API.Controllers.Author;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Explorer.Payment.Tests.Integration.Shopping
{
    [Collection("Sequential")]
    public class SaleTourTest : BasePaymentIntegrationTest
    {
        public SaleTourTest(PaymentTestFactory factory) : base(factory) { }

        [Fact]
        public void Create_sale_successfully()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

            var saleDto = new TourSaleDto
            {
                Name = "TestSale1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                DiscountPercentage = 20.0,
                Tours = new List<TourDtoPayment>()
            };

            var result = ((ObjectResult)controller.CreateTourSale(saleDto).Result).Value as TourSaleDto;

            result.ShouldNotBeNull();

            var storedSales = dbContext.TourSales.FirstOrDefault(b => b.Name == "TestSale1");
            storedSales.ShouldNotBeNull();
            storedSales.DiscountPercentage.ShouldBe(20.0);
            storedSales.Tours.ShouldBeEmpty();
        }

        [Fact]
        public void Update_sale_successfully()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

            var existingSale = dbContext.TourSales.FirstOrDefault(b => b.Id == -1);
            existingSale.ShouldNotBeNull();

            var updateDto = new TourSaleDto
            {
                Id = existingSale.Id,
                Name = "UpdatedSale",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(3),
                DiscountPercentage = 15.0
            };

            var result = ((ObjectResult)controller.UpdateTourSale(existingSale.Id, updateDto).Result).Value as TourSaleDto;

            result.ShouldNotBeNull();
            result.Name.ShouldBe("UpdatedSale");
            result.DiscountPercentage.ShouldBe(15.0);

            var updatedSale = dbContext.TourSales.FirstOrDefault(b => b.Id == existingSale.Id);
            updatedSale.ShouldNotBeNull();
            updatedSale.Name.ShouldBe("UpdatedSale");
        }

        [Fact]
        public void Delete_sale_successfully()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

            var existingSale = dbContext.TourSales.FirstOrDefault(b => b.Id == -2);
            existingSale.ShouldNotBeNull();

            var result = ((ObjectResult)controller.DeleteTourSale(existingSale.Id).Result);

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            var deletedSale = dbContext.TourSales.FirstOrDefault(b => b.Id == existingSale.Id);
            deletedSale.ShouldBeNull();
        }

        [Fact]
        public void Create_sale_fails_with_invalid_data()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");

            var invalidSaleDto = new TourSaleDto { Name = "", DiscountPercentage = -5.0 };

            var result = ((ObjectResult)controller.CreateTourSale(invalidSaleDto).Result);

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Update_sale_fails_with_nonexistent_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");

            var updateDto = new TourSaleDto { Name = "NonexistentSale", DiscountPercentage = 15.0 };

            var result = ((ObjectResult)controller.UpdateTourSale(-999, updateDto).Result);

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Delete_sale_fails_with_nonexistent_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");

            var result = ((ObjectResult)controller.DeleteTourSale(-999).Result);

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        private static TourSaleController CreateController(IServiceScope scope, string userId)
        {
            return new TourSaleController(scope.ServiceProvider.GetRequiredService<ITourSaleService>())
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
}

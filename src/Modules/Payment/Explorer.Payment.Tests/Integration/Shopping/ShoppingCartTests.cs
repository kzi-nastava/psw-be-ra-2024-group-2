using Explorer.API.Controllers.Tourist;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payment.Tests.Integration.Shopping;

[Collection("Sequential")]
public class ShoppingCartTests : BasePaymentIntegrationTest
{
    public ShoppingCartTests(PaymentTestFactory factory) : base(factory) { }

    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        // Act
        var result = (ObjectResult)controller.RemoveItemFromCart(-1).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.OrderItems.OfType<TourOrderItem>().FirstOrDefault(i => i.TourId == -1);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Adds()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        // Act
        var result = (ObjectResult)controller.AddItemToCart(-6).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.OrderItems.OfType<TourOrderItem>().FirstOrDefault(i => i.TourId == -2);
        storedCourse.ShouldNotBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = (ObjectResult)controller.RemoveItemFromCart(-1000).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    //[Fact]
    //public void AddsBundle()
    //{
    //    // Arrange
    //    using var scope = Factory.Services.CreateScope();
    //    var controller = CreateController(scope);
    //    var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

    //    var bundleOrderItemDto = new BundleOrderItemDto
    //    {
    //        BundleId = -2,
    //        TourIds = new List<int> { -3, -4 },
    //        Price = 100
    //    };

    //    // Act
    //    var result = (ObjectResult)controller.AddBundleToCart(-21, bundleOrderItemDto).Result;

    //    // Assert - Response
    //    result.ShouldNotBeNull();
    //    result.StatusCode.ShouldBe(200);

    //    // Assert - Database
    //    var storedBundle = dbContext.OrderItems.OfType<BundleOrderItem>()
    //                          .FirstOrDefault(i => i.BundleId == -2 && i.UserId == -21);
    //    storedBundle.ShouldNotBeNull();
    //    storedBundle.TourIds.ShouldBe(bundleOrderItemDto.TourIds);
    //    storedBundle.Price.ShouldBe(100);
    //}

    private static ShoppingCartController CreateController(IServiceScope scope)
    {
        return new ShoppingCartController(scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
        {
            ControllerContext = BuildContext("-21")
        };
    }
}

using Explorer.API.Controllers.Tourist;
using Explorer.Payment.API.Public.Tourist;
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
        var result = (OkResult)controller.RemoveItemFromCart(-1);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.OrderItems.FirstOrDefault(i => i.TourId == -1);
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
        var result = (OkResult)controller.AddItemToCart(-2);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.OrderItems.FirstOrDefault(i => i.TourId == -2);
        storedCourse.ShouldNotBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);


        // Act & Assert
        var exception = Should.Throw<NullReferenceException>(() => controller.RemoveItemFromCart(-1000));
        exception.Message.ShouldContain("Object reference not set to an instance of an object.");
    }


    private static ShoppingCartController CreateController(IServiceScope scope)
    {
        return new ShoppingCartController(scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
        {
            ControllerContext = BuildContext("-21")
        };
    }
}

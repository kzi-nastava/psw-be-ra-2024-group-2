using Explorer.API.Controllers.Author;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Payment.Tests.Integration.Shopping;

[Collection("Sequential")]
public class TourBundlesTests : BasePaymentIntegrationTest
{
    public TourBundlesTests(PaymentTestFactory factory) : base(factory) { }

    [Fact]
    public void CreatesBundle()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var bundleDto = new BundleDto
        {
            Name = "Test Bundle",
            Price = 100.0,
            Tours = new List<BundleItemDto>()
        };

        // Act
        var result = ((OkObjectResult)controller.CreateBundle(bundleDto).Result).Value as BundleDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedBundle = dbContext.TourBundles.FirstOrDefault(b => b.Name == "Test Bundle");
        storedBundle.ShouldNotBeNull();
        storedBundle.Price.ShouldBe(100.0);
        storedBundle.Tours.ShouldBeEmpty();
    }

    [Fact]
    public void PublishesBundle()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var bundle = new TourBundle(-21, "Bundle to Publish", 200.0, BundleStatus.Draft)
        {
            Tours = new List<int>
            {
                -6, -7
            }
        };
        dbContext.TourBundles.Add(bundle);
        dbContext.SaveChanges();

        // Act
        var result = (ObjectResult)controller.PublishBundle(bundle.Id).Result;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var updatedBundle = dbContext.TourBundles.FirstOrDefault(b => b.Id == bundle.Id);
        updatedBundle.ShouldNotBeNull();
        updatedBundle.Status.ShouldBe(BundleStatus.Published);
    }

    [Fact]
    public void PublishBundle_Fails_NotEnoughTours()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var bundle = new TourBundle(-21, "Incomplete Bundle", 150.0, BundleStatus.Draft)
        {
            Tours = new List<int>
            {
                1
            }
        };
        dbContext.TourBundles.Add(bundle);
        dbContext.SaveChanges();

        var result = (ObjectResult)controller.PublishBundle(bundle.Id).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(500);
    }

    [Fact]
    public void DeletesBundle()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var bundle = new TourBundle(-21, "Bundle to Delete", 100.0, BundleStatus.Draft);
        dbContext.TourBundles.Add(bundle);
        dbContext.SaveChanges();

        // Act
        var result = ((OkObjectResult)controller.DeleteBundle(bundle.Id).Result).Value as BundleDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var deletedBundle = dbContext.TourBundles.FirstOrDefault(b => b.Id == bundle.Id);
        deletedBundle.ShouldBeNull();
    }

    private static BundleController CreateController(IServiceScope scope, string userId)
    {
        return new BundleController(scope.ServiceProvider.GetRequiredService<IBundleService>())
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
using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Infrastructure.Database;
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

public class TourSouvenirTests : BasePaymentIntegrationTest
{
    public TourSouvenirTests(PaymentTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates_Successfully()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenirDto = new TourSouvenirDto
        {
            Name = "Test Souvenir",
            Price = 100.0,
            Description = "Test Description",
            Count = 15,
            ImageDto = new PaymentImageDto
            {
                Data = "Some data...",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            },
            TourId = -6
        };

        // Act
        var result = ((OkObjectResult)controller.CreateSouvenir(souvenirDto).Result).Value as TourSouvenirDto;


        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedSouvenir = dbContext.TourSouvenirs.FirstOrDefault(s => s.Name == "Test Souvenir");
        storedSouvenir.ShouldNotBeNull();
        storedSouvenir.Price.ShouldBe(100.0);
    }

    [Fact]
    public void Creates_Unsuccessfully_No_Tour_Valid()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenirDto = new TourSouvenirDto
        {
            Name = "Test Souvenir",
            Price = 100.0,
            Description = "Test Description",
            Count = 15,
            ImageDto = new PaymentImageDto
            {
                Data = "Some data...",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            },
            TourId = -1000
        };

        // Act
        var result = (ObjectResult)controller.CreateSouvenir(souvenirDto).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void Creates_Unsuccessfully_Authors_Dont_Match()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-2");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenirDto = new TourSouvenirDto
        {
            Name = "Test Souvenir",
            Price = 100.0,
            Description = "Test Description",
            Count = 15,
            ImageDto = new PaymentImageDto
            {
                Data = "Some data...",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            },
            TourId = -6
        };

        // Act
        var result = (ObjectResult)controller.CreateSouvenir(souvenirDto).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public void Delete_Successfully_Draft_Souvenir()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenir = new TourSouvenir("Test Souvenir 1", 100.0, "Test Description", 15, SouvenirStatus.Draft, new Image("Some data...", DateTime.UtcNow, "image/jpeg"), -6, -1);
        dbContext.TourSouvenirs.Add(souvenir);
        dbContext.SaveChanges();

        // Act
        var result = ((OkObjectResult)controller.DeleteSouvenir(souvenir.Id).Result).Value as TourSouvenirDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedSouvenir = dbContext.TourSouvenirs.FirstOrDefault(s => s.Name == "Test Souvenir 1");
        storedSouvenir.ShouldBeNull();
    }

    [Fact]
    public void Delete_Successfully_Archives_Published_Souvenir()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenir = new TourSouvenir("Test Souvenir 2", 100.0, "Test Description", 15, SouvenirStatus.Published, new Image("Some data...", DateTime.UtcNow, "image/jpeg"), -6, -1);
        dbContext.TourSouvenirs.Add(souvenir);
        dbContext.SaveChanges();

        // Act
        var result = ((OkObjectResult)controller.DeleteSouvenir(souvenir.Id).Result).Value as TourSouvenirDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedSouvenir = dbContext.TourSouvenirs.FirstOrDefault(s => s.Name == "Test Souvenir 2");
        storedSouvenir.ShouldNotBeNull();
        storedSouvenir.SouvenirStatus.ShouldBe(SouvenirStatus.Archived);
    }

    [Fact]
    public void Publish_Successfully()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenir = new TourSouvenir("Test Souvenir 3", 100.0, "Test Description", 15, SouvenirStatus.Draft, new Image("Some data...", DateTime.UtcNow, "image/jpeg"), -6, -1);
        dbContext.TourSouvenirs.Add(souvenir);
        dbContext.SaveChanges();

        // Act
        var result = ((OkObjectResult)controller.PublishSouvenir(souvenir.Id).Result).Value as TourSouvenirDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedSouvenir = dbContext.TourSouvenirs.FirstOrDefault(s => s.Name == "Test Souvenir 3");
        storedSouvenir.ShouldNotBeNull();
        storedSouvenir.SouvenirStatus.ShouldBe(SouvenirStatus.Published);
    }

    [Fact]
    public void Publish_Unsuccessfully_Not_Author()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-2");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenir = new TourSouvenir("Test Souvenir 4", 100.0, "Test Description", 15, SouvenirStatus.Draft, new Image("Some data...", DateTime.UtcNow, "image/jpeg"), -6, -1);
        dbContext.TourSouvenirs.Add(souvenir);
        dbContext.SaveChanges();

        // Act
        var result = (ObjectResult)controller.PublishSouvenir(souvenir.Id).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public void Publish_Unsuccessfully_Already_Published()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenir = new TourSouvenir("Test Souvenir 5", 100.0, "Test Description", 15, SouvenirStatus.Published, new Image("Some data...", DateTime.UtcNow, "image/jpeg"), -6, -1);
        dbContext.TourSouvenirs.Add(souvenir);
        dbContext.SaveChanges();

        // Act
        var result = (ObjectResult)controller.PublishSouvenir(souvenir.Id).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public void Update_Successfully()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenir = new TourSouvenir("Test Souvenir 6", 100.0, "Test Description", 15, SouvenirStatus.Draft, new Image("Some data...", DateTime.UtcNow, "image/jpeg"), -6, -1);
        dbContext.TourSouvenirs.Add(souvenir);
        dbContext.SaveChanges();

        var souvenirDto = new TourSouvenirDto
        {
            Id = souvenir.Id,
            Name = "Updated Souvenir",
            Price = 200.0,
            Description = "Updated Description",
            Count = 20,
            ImageDto = new PaymentImageDto
            {
                Data = "Updated data...",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            },
            TourId = -6
        };

        // Act
        var result = ((OkObjectResult)controller.UpdateSouvenir(souvenirDto).Result).Value as TourSouvenirDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedSouvenir = dbContext.TourSouvenirs.FirstOrDefault(s => s.Name == "Updated Souvenir");
        storedSouvenir.ShouldNotBeNull();
        storedSouvenir.Price.ShouldBe(200.0);
    }

    [Fact]
    public void Update_Unsuccessfully_Published_Souvenir_Cant_Be_Updated()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var souvenir = new TourSouvenir("Test Souvenir 7", 100.0, "Test Description", 15, SouvenirStatus.Published, new Image("Some data...", DateTime.UtcNow, "image/jpeg"), -6, -1);
        dbContext.TourSouvenirs.Add(souvenir);
        dbContext.SaveChanges();

        var souvenirDto = new TourSouvenirDto
        {
            Id = souvenir.Id,
            Name = "Updated Souvenir",
            Price = 200.0,
            Description = "Updated Description",
            Count = 20,
            ImageDto = new PaymentImageDto
            {
                Data = "Updated data...",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            },
            TourId = -6
        };

        // Act
        var result = (ObjectResult)controller.UpdateSouvenir(souvenirDto).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
    }

        private static TourSouvenirController CreateController(IServiceScope scope, string userId)
    {
        return new TourSouvenirController(scope.ServiceProvider.GetRequiredService<ITourSouvenirService>())
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

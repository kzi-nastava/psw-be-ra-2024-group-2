using Explorer.API.Controllers.Administrator;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Infrastructure.Database;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Payment.Tests.Integration.Administration;

[Collection("Sequential")]
public class AdministratorWalletTests : BasePaymentIntegrationTest
{
    public AdministratorWalletTests(PaymentTestFactory factory) : base(factory) { }

    [Fact]
    public void RetrievesWalletBalance()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "admin");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var wallet = new Wallet(-42)
        {
            AdventureCoinsBalance = 500.0
        };

        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        // Act
        var actionResult = controller.GetWalletBalance(-42);
        var result = (actionResult as OkObjectResult)?.Value as WalletDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(-42);
        result.AdventureCoinsBalance.ShouldBe(500.0);
    }
    [Fact]
    public void AddsAdventureCoins()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "admin");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        // Act
        var actionResult = controller.AddAdventureCoins(-42, 200);

        // Assert - Response
        var okResult = actionResult as OkResult;
        okResult.ShouldNotBeNull("Response should be an OkResult, indicating success.");

        // Assert - Database
        var updatedWallet = dbContext.Wallets.FirstOrDefault(w => w.UserId == -42);
        updatedWallet.ShouldNotBeNull("Wallet should exist in the database after test execution.");
        updatedWallet.AdventureCoinsBalance.ShouldBe(200.0, "Wallet balance should reflect the added funds.");
    }



    private static AdministratorWalletController CreateController(IServiceScope scope, string userId)
    {
        return new AdministratorWalletController(scope.ServiceProvider.GetRequiredService<IWalletService>())
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("id", userId),
                        new Claim("role", "administrator")
                    }))
                }
            }
        };
    }
}

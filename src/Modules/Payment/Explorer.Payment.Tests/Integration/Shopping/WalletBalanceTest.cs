using Explorer.API.Controllers.Tourist;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Payment.Tests.Integration.Shopping;

[Collection("Sequential")]
public class WalletBalanceTest : BasePaymentIntegrationTest
{
    public WalletBalanceTest(PaymentTestFactory factory) : base(factory) { }

    [Fact]
    public async Task RetrievesWalletBalance()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-42");
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var wallet = new Wallet(-42)
        {
            AdventureCoinsBalance = 250.0
        };

        dbContext.Wallets.Add(wallet);
        dbContext.SaveChanges();

        // Act
        var actionResult = controller.GetWalletBalance();
        var result = actionResult as OkObjectResult;
        var walletDto = result?.Value as WalletDto;

        // Assert - Response
        walletDto.ShouldNotBeNull();
        walletDto.UserId.ShouldBe(-42);
        walletDto.AdventureCoinsBalance.ShouldBe(250.0);
    }



    private static TouristWalletController CreateController(IServiceScope scope, string userId)
    {
        return new TouristWalletController(scope.ServiceProvider.GetRequiredService<IWalletService>())
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

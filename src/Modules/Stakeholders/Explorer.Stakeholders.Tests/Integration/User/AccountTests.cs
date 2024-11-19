using Explorer.API.Controllers;
using Explorer.API.Controllers.Administrator;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Infrastructure.Database;
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

namespace Explorer.Stakeholders.Tests.Integration.User
{
    [Collection("Sequential")]
    public class AccountTests : BaseStakeholdersIntegrationTest
    {
        public AccountTests(StakeholdersTestFactory factory) : base(factory) { }


        [Fact]
        public void Get_successfully_returns_accounts()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");

            // Act
            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<AccountDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(11);
            result.TotalCount.ShouldBe(11);
        }

        [Fact]
        public void Successfully_block_account()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var updatedEntity = new AccountDto
            {
                UserId = -11,
                Username = "autor1@gmail.com",
                Email = "autor1@gmail.com",
                Role = UserRole.Author,
                IsBlocked = true
            };

            // Act
            var result = ((ObjectResult)controller.Block(updatedEntity).Result)?.Value as AccountDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Username.ShouldBe(updatedEntity.Username);
            result.Email.ShouldBe(updatedEntity.Email);
            result.Role.ShouldBe(updatedEntity.Role);
            result.IsBlocked.ShouldBe(updatedEntity.IsBlocked);

            // Assert - Database
            var storedEntity = dbContext.Users.FirstOrDefault(i => i.Id == updatedEntity.UserId 
                && i.IsBlocked == updatedEntity.IsBlocked);
            storedEntity.ShouldNotBeNull();
            var oldEntity = dbContext.Users.FirstOrDefault(i => i.Id == updatedEntity.UserId
                && i.IsBlocked != updatedEntity.IsBlocked);
            oldEntity.ShouldBeNull();
        }

        [Fact]
        public void Unsuccessfully_block_account() //non existent user
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1"); 
            var nonExistentEntity = new AccountDto
            {
                UserId = -5,
                Username = "autor1@gmail.com",
                Email = "autor1@gmail.com",
                Role = UserRole.Author,
                IsBlocked = true
            };

            // Act
            var result = ((ObjectResult)controller.Block(nonExistentEntity).Result)?.Value as AccountDto;

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Successfully_unblock_account()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var updatedEntity = new AccountDto
            {
                UserId = -11,
                Username = "autor1@gmail.com",
                Email = "autor1@gmail.com",
                Role = UserRole.Author,
                IsBlocked = false
            };

            // Act
            var result = ((ObjectResult)controller.Unblock(updatedEntity).Result)?.Value as AccountDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Username.ShouldBe(updatedEntity.Username);
            result.Email.ShouldBe(updatedEntity.Email);
            result.Role.ShouldBe(updatedEntity.Role);
            result.IsBlocked.ShouldBe(updatedEntity.IsBlocked);

            // Assert - Database
            var storedEntity = dbContext.Users.FirstOrDefault(i => i.Id == updatedEntity.UserId
                && i.IsBlocked == updatedEntity.IsBlocked);
            storedEntity.ShouldNotBeNull();
            var oldEntity = dbContext.Users.FirstOrDefault(i => i.Id == updatedEntity.UserId
                && i.IsBlocked != updatedEntity.IsBlocked);
            oldEntity.ShouldBeNull();
        }

        [Fact]
        public void Unsuccessfully_unblock_account() //non existent user
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var nonExistentEntity = new AccountDto
            {
                UserId = -5,
                Username = "autor1@gmail.com",
                Email = "autor1@gmail.com",
                Role = UserRole.Author,
                IsBlocked = false
            };

            // Act
            var result = ((ObjectResult)controller.Unblock(nonExistentEntity).Result)?.Value as AccountDto;

            // Assert
            result.ShouldBeNull();
        }


        private static AccountController CreateController(IServiceScope scope, string number)
        {
            return new AccountController(scope.ServiceProvider.GetRequiredService<IAccountService>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                        new Claim("personId", number)
                    }))
                    }
                }
            };
        }
        private static AccountController CreateController(IServiceScope scope)
        {
            return new AccountController(scope.ServiceProvider.GetRequiredService<IAccountService>());
        }
    }
}

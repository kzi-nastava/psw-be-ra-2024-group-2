using Explorer.API.Controllers;
using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Explorer.API.Controllers.Tourist;
using Explorer.API.Controllers.Author;
using Explorer.Stakeholders.Core.UseCases;

namespace Explorer.Stakeholders.Tests.Integration.RatingApp
{
    [Collection("Sequential")]
    public class RatingApplicationTests : BaseStakeholdersIntegrationTest
    {
         public RatingApplicationTests(StakeholdersTestFactory factory) : base(factory) { }

        private static RatingApplicationAdministratorController CreateControllerReview(IServiceScope scope, string userId)
        {
            var ratingApplicationService = scope.ServiceProvider.GetRequiredService<IRatingApplicationService>();
            var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();

            return new RatingApplicationAdministratorController(ratingApplicationService, personService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                    new Claim("userId", userId)
                }))
                    }
                }
            };
        }
        private static RatingApplicationTouristController CreateControllerTourist(IServiceScope scope, string userId)
        {
            var ratingApplicationService = scope.ServiceProvider.GetRequiredService<IRatingApplicationService>();
            var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();

            return new RatingApplicationTouristController(ratingApplicationService, personService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                    new Claim("userId", userId)
                }))
                    }
                }
            };
        }

        private static RatingApplicationAuthorController CreateControllerAuthor(IServiceScope scope, string userId)
        {
            var ratingApplicationService = scope.ServiceProvider.GetRequiredService<IRatingApplicationService>();
            var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();

            return new RatingApplicationAuthorController(ratingApplicationService, personService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                    new Claim("userId", userId)
                }))
                    }
                }
            };
        }


        [Fact]
        public void Admin_review_all_ratings()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateControllerReview(scope, "-1");

            // Act
            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<RatingWithUserDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(5);
            result.TotalCount.ShouldBe(5);   //jer imas 3 dodata u bazi preko insert ... i jos ova 2 dodajes to je 5
        }

        [Fact]
         public void CreateRate_successfully_saves_tourists_rating_application_()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var controller = CreateControllerTourist(scope, "-1");

            var newRating = new RatingApplicationDto
            {
                Grade = 3,
                Comment = "Tourists comment.",
                RatingTime = DateTime.UtcNow,
                UserId = -11
            };

            // Act
            var result = ((ObjectResult)controller.Create(newRating).Result)?.Value as RatingApplicationDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Grade.ShouldBe(newRating.Grade);
            result.Comment.ShouldBe(newRating.Comment);
            result.UserId.ShouldBe(-11);

            // Assert - Database
            var storedRating = dbContext.RatingsApplication.FirstOrDefault(r => r.UserId == result.UserId);
            storedRating.ShouldNotBeNull();
            storedRating.Grade.ShouldBe(newRating.Grade);
            storedRating.UserId.ShouldBe(-11);
        }

        [Fact]
        public void CreateRate_successfully_saves_authors_rating_application_()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var controller = CreateControllerAuthor(scope, "-21");

            var newRating = new RatingApplicationDto
            {
                Grade = 2,
                Comment = "Authors comment.",
                RatingTime = DateTime.UtcNow,
                UserId = -21
            };

            // Act
            var result = ((ObjectResult)controller.Create(newRating).Result)?.Value as RatingApplicationDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Grade.ShouldBe(newRating.Grade);
            result.Comment.ShouldBe(newRating.Comment);
            result.UserId.ShouldBe(-21);

            // Assert - Database
            var storedRating = dbContext.RatingsApplication.FirstOrDefault(r => r.UserId == result.UserId);
            storedRating.ShouldNotBeNull();
            storedRating.Grade.ShouldBe(newRating.Grade);
            storedRating.UserId.ShouldBe(-21);
        }




        [Fact]
        public void CreateRate_fails_when_UserId_already_exists()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var controller = CreateControllerTourist(scope, "3"); // Pretpostavimo da korisnik sa UserId 3 već postoji

            // Dodajemo ocenu koja već postoji za korisnika sa UserId = 3
            var existingRating = new RatingApplicationDto
            {
                Grade = 5,
                Comment = "Neki kom3",
                RatingTime = DateTime.UtcNow,
                UserId = -3
            };

            // Proveravamo da li već postoji ocena za korisnika sa UserId = 3
            var existingUser = dbContext.RatingsApplication.FirstOrDefault(r => r.UserId == existingRating.UserId);
            existingUser.ShouldNotBeNull(); // Proveravamo da već postoji

            var newRating = new RatingApplicationDto
            {
                Grade = 1,
                Comment = "Trying to create a new rating for the same user.",
                RatingTime = DateTime.UtcNow,
                UserId = -3
            };

            // Act
            var result = ((ObjectResult)controller.Create(newRating).Result)?.Value as RatingApplicationDto;

            // Assert - Proveravamo da je kreiranje ocene neuspešno jer korisnik već postoji
            result.ShouldBeNull(); // Kreiranje bi trebalo da vrati grešku jer korisnik već postoji

            // Proveravamo da baza nije sačuvala novu ocenu za već postojećeg korisnika
            var storedRating = dbContext.RatingsApplication.FirstOrDefault(r => r.UserId == newRating.UserId && r.Grade == newRating.Grade && r.Comment == newRating.Comment);
            storedRating.ShouldBeNull(); // Provera da nova ocena nije sačuvana
        }

    }
}


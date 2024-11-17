using Explorer.API.Controllers;
using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Tour
{
    [Collection("Sequential")]
    public class CreateTourPreferenceTests : BaseToursIntegrationTest
    {
        public CreateTourPreferenceTests(ToursTestFactory factory) : base(factory) { }
        
        [Fact]
        public void Successfully_created_tour_preference()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new TourPreferenceDto
            {
                TouristId = -22,
                Difficulty = "Easy",
                WalkRating = 2,
                BicycleRating = 2,
                BoatRating = 2,
                CarRating = 2,
                Tags = new List<string>()
            };


            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourPreferenceDto;

            result.ShouldNotBeNull();
        }

        private static TourPreferenceController CreateController(IServiceScope scope)
        {
            return new TourPreferenceController(scope.ServiceProvider.GetRequiredService<ITourPreferenceService>())
            {
                ControllerContext = BuildContext("-22")
            };
        }
    }
}

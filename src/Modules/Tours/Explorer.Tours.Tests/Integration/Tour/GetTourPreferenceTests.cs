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
    public class GetTourPreferenceTests : BaseToursIntegrationTest
    {
        public GetTourPreferenceTests(ToursTestFactory factory) : base(factory) { }
        
        [Fact]
        public void Successfully_got_tour_preference()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            long id = -1; 
            
            var result = ((ObjectResult)controller.GetByTouristId(id).Result)?.Value as TourPreferenceDto;

            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);
        }

        private static TourPreferenceController CreateController(IServiceScope scope)
        {
            return new TourPreferenceController(scope.ServiceProvider.GetRequiredService<ITourPreferenceService>());
        }
    }
}

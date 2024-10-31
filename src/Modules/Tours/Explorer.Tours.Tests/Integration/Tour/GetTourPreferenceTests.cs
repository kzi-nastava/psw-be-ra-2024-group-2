using Explorer.API.Controllers;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;


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
            
            var result = ((ObjectResult)controller.GetByTouristId().Result)?.Value as PagedResult<TourPreferenceDto>;

            result.ShouldNotBeNull();
            result.Results.First().Id.ShouldBe(-1);
        }

        private static TourPreferenceController CreateController(IServiceScope scope)
        {
            return new TourPreferenceController(scope.ServiceProvider.GetRequiredService<ITourPreferenceService>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                    new Claim("personId", "-21"),
                    new Claim("id", "-21")
                }))
                    }
                }
            };
        }

    }
}

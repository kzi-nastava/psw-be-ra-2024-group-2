using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
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

namespace Explorer.Tours.Tests.Integration.Tour
{
    [Collection("Sequential")]

    public class CheckpointTests : BaseToursIntegrationTest
    {
        public CheckpointTests(ToursTestFactory factory) : base(factory)
        {}

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var newEntity = new CheckpointDto
            {
                Longitude = 10,
                Latitude = 20,
                Image = new TourImageDto
                {
                    Data = "coa testna slika",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/png"
                },
                Name = "Trg Nikole Pasica",
                Description = "Testni opis2"

            };
            // Act
            var result = (ObjectResult)controller.Create(newEntity).Result;

            //Assert - Response
            result.StatusCode.ShouldBe(200);

        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var updatedEntity = new CheckpointDto
            {
                Longitude = 10,
                Latitude = 20,
                Image = new TourImageDto
                {
                    Data = "coa testna slika2",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Name = "",
                Description = "Testni opis"
            };

            //Act
            var result = (ObjectResult)controller.Create(updatedEntity).Result;

            //Assert - Response
            result.StatusCode.ShouldBe(409);
        }

        private static CheckpointController CreateController(IServiceScope scope, string number)
        {
            return new CheckpointController(scope.ServiceProvider.GetRequiredService<ICheckpointService>())
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
    }
}

using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Tour
{
    [Collection("Sequential")]
    public class EventTests : BaseToursIntegrationTest
    {
        public EventTests(ToursTestFactory factory) : base(factory) { }




        [Fact]
        public void Create_fails_name_null()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new EventDto
            {
                Category = "Concert",
                Description = "Opis dogadjaja",
                Image = new EventImageDto
                {
                    Data = "new data for this idkksasakkk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Longitude = 0,
                Latitude = 0,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
            };

            // Act
            var result = (ObjectResult)controller.CreateEvent(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Create_fails_name_empty()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new EventDto
            {
                Name = "",
                Category = "Concert",
                Description = "opis",
                Image = new EventImageDto
                {
                    Data = "new data for tttasstthis idk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Longitude = 0,
                Latitude = 0
            };

            // Act
            var result = (ObjectResult)controller.CreateEvent(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Create_fails_category_null()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new EventDto
            {
                Name = "Concert",
                Description = "opiss",
                Image = new EventImageDto
                {
                    Data = "new daaaata for thisass idk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Longitude = 0,
                Latitude = 0
            };

            // Act
            var result = (ObjectResult)controller.CreateEvent(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Create_fails_category_non_existant()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new EventDto
            {
                Name = "Concert",
                Category = "Shopping centre",
                Description = "opis",
                Image = new EventImageDto
                {
                    Data = "new datttsasata for this idk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Longitude = 0.0,
                Latitude = 0.0
            };

            // Act
            var result = (ObjectResult)controller.CreateEvent(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);

        }
        private static EventController CreateController(IServiceScope scope)
        {
            return new EventController(scope.ServiceProvider.GetRequiredService<IEventService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

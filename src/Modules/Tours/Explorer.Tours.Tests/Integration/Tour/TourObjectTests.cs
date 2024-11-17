using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
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
    public class TourObjectTests : BaseToursIntegrationTest
    {
        public TourObjectTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Create_object_successful()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new ObjectDto
            {
                Name = "Restoran Dva sesira",
                Description = "Restoran na Skadarliji, poznat po ukusnim specjalitetima domace kuhinje.",
                Category = "Restaurant",
                Longitude = 0.0,
                Latitude = 0.0,
                Image = new ObjectImageDto
                {
                    Data = "new data for this jjhkjhjghfhgfhgdffgsfdsidk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                }
                


            };


            //Act
            var result = ((ObjectResult)controller.CreateObject(newEntity).Result)?.Value as ObjectDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Name.ShouldBe(newEntity.Name);

            // Assert - Database
            var storedEntity = dbContext.Objects.FirstOrDefault(i => i.Name == newEntity.Name);
            storedEntity.ShouldNotBeNull();


        }

        [Fact]
        public void Create_fails_name_null()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new ObjectDto
            {
                Category = "Restaurant",
                Description = "Restoran na Skadarliji, poznat po ukusnim specjalitetima domace kuhinje.",
                Image = new ObjectImageDto
                {
                    Data = "new data for this idkkkkk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Longitude = 0,
                Latitude = 0
            };

            // Act
            var result = (ObjectResult)controller.CreateObject(updatedEntity).Result;

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
            var updatedEntity = new ObjectDto
            {
                Name = "",
                Category = "Restaurant",
                Description = "Restoran na Skadarliji, poznat po ukusnim specjalitetima domace kuhinje.",
                Image = new ObjectImageDto
                {
                    Data = "new data for ttttthis idk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Longitude= 0,
                Latitude= 0
            };

            // Act
            var result = (ObjectResult)controller.CreateObject(updatedEntity).Result;

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
            var updatedEntity = new ObjectDto
            {
                Name = "Restaurant",
                Description = "Restoran na Skadarliji, poznat po ukusnim specjalitetima domace kuhinje.",
                Image = new ObjectImageDto
                {
                    Data = "new daaaata for this idk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Longitude = 0,
                Latitude = 0
            };

            // Act
            var result = (ObjectResult)controller.CreateObject(updatedEntity).Result;

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
            var updatedEntity = new ObjectDto
            {
                Name = "Shopping centre",
                Category = "Shopping centre",
                Description = "Restoran na Skadarliji, poznat po ukusnim specjalitetima domace kuhinje.",
                Image = new ObjectImageDto
                {
                    Data = "new datttta for this idk",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                Longitude= 0.0,
                Latitude= 0.0
            };

            // Act
            var result = (ObjectResult)controller.CreateObject(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);

        }
        private static ObjectController CreateController(IServiceScope scope)
        {
            return new ObjectController(scope.ServiceProvider.GetRequiredService<IObjectService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

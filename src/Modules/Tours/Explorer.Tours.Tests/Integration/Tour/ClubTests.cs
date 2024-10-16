using Explorer.API.Controllers.Administrator.Administration;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Tour
{
    [Collection("Sequential")]
    public class ClubTests : BaseToursIntegrationTest
    {
        public ClubTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Create_club_successfully_test()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new ClubDto()
            {
                Name = "Novi Sad",
                Description = "Jedan od najboljih turistickih klubova.",
                ImageId = 1,
                OwnerId = 1
            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ClubDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Name.ShouldBe(newEntity.Name);
            result.Description.ShouldBe(newEntity.Description);
            result.ImageId.ShouldBe(newEntity.ImageId);

            // Assert - Database
            var storedEntity = dbContext.Clubs.FirstOrDefault(i => i.Name == newEntity.Name);
            storedEntity.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateClub_successfully_test()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var updateEntity = new ClubDto()
            {
                Name = "Beograd",
                Description = "Jedan od najboljih turistickih klubova.",
                ImageId = 1,
                OwnerId = 1
            };

            // Act
            var result = ((ObjectResult)controller.Update(1,updateEntity).Result)?.Value as ClubDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Name.ShouldBe(updateEntity.Name);
            result.Description.ShouldBe(updateEntity.Description);
            result.ImageId.ShouldBe(updateEntity.ImageId);
            result.OwnerId.ShouldBe(updateEntity.OwnerId);
            
            // Assert - Database
            var storedEntity = dbContext.Clubs.FirstOrDefault(i => i.Name == "Beograd");
            storedEntity.ShouldNotBeNull();
            storedEntity.Description.ShouldBe(updateEntity.Description);
            var oldEntity = dbContext.Equipment.FirstOrDefault(i => i.Name == "Novi Sad");
            oldEntity.ShouldBeNull();
        }

        [Fact]
        public void Create_club_invalid_values_should_return_BadRequest()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var invalidEntity = new ClubDto()
            {
                Name = "", 
                Description = "",
                ImageId = 1,
                OwnerId = 1
            };

            // Act
            var result = (ObjectResult)controller.Create(invalidEntity).Result;

            // Assert
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Update_club_invalid_values_should_return_BadRequest()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var invalidEntity = new ClubDto()
            {
                Name = "",
                Description = "",
                ImageId = 1,
                OwnerId = 1,
            };

            // Act
            var result = (ObjectResult)controller.Update(1, invalidEntity).Result;

            // Assert
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Update_club_non_existing_id_should_return_NotFound()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var updateEntity = new ClubDto()
            {
                Name = "Valid Name",
                Description = "Valid Description",
                ImageId = 1,
                OwnerId = 1
            };

            // Act
            var result = (ObjectResult)controller.Update(-999, updateEntity).Result;

            // Assert
            result.StatusCode.ShouldBe(404);
        }


        private static ClubController CreateController(IServiceScope scope)
        {
            return new ClubController(scope.ServiceProvider.GetRequiredService<IClubService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

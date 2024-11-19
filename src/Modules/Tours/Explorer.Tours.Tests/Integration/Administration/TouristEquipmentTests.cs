using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.Infrastructure.Database;
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

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TouristEquipmentTests : BaseToursIntegrationTest
    {
        public TouristEquipmentTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Act
            var result = (ObjectResult)controller.RemoveEquipmentFromTourist(-1);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.TouristEquipments.FirstOrDefault(i => i.EquipmentId == -1);
            storedCourse.ShouldBeNull();
        }

        [Fact]
        public void Adds()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Act
            var result = (ObjectResult)controller.AddEquipmentToTourist(-2);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.TouristEquipments.FirstOrDefault(i => i.EquipmentId == -2);
            storedCourse.ShouldNotBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.RemoveEquipmentFromTourist(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(500);
        }

        private static TouristEquipmentController CreateController(IServiceScope scope)
        {
            return new TouristEquipmentController(scope.ServiceProvider.GetRequiredService<IEquipmentService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

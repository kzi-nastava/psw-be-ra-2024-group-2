using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Tours.Tests.Integration.Tour;

[Collection("Sequential")]
public class TourTests : BaseToursIntegrationTest
{
    public TourTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void UpdateEquipment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto
        {
            Id = -1,
            UserId = -1,
            Equipment = {-1, -3}
        
        };

        //Act
        var result = ((ObjectResult)controller.UpdateEquipment(newEntity).Result)?.Value as TourDto;

        //Assert - Response
        result.Id.ShouldBe(-1);

        //Assert - Database
        var storedEntity = dbContext.Tours
            .Where(i => i.Id == newEntity.Id)
            .ToList()
            .FirstOrDefault(i => IsEquipmentEqual(i.Equipment, newEntity.Equipment));

        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void AddAlreadyInsertedEquipment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-2");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var equipment = dbContext.Equipment.FirstOrDefault(i => i.Id == -1);
        var tour = dbContext.Tours.Include(t => t.Equipment).FirstOrDefault(t => t.Id == -3);


        tour.Equipment.Add(equipment);
        dbContext.SaveChanges();

        var newEntity = new TourDto
        {
            Id = -3,
            UserId = -2,
            Equipment = { -1 }

        };

        //Act
        var result = (ObjectResult)controller.UpdateEquipment(newEntity).Result;

        //Assert - Response
        result.StatusCode.ShouldBe(400);

    }

    [Fact]
    public void FakeUserAddEquipment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-2");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto
        {
            Id = -1,
            UserId = -1,
            Equipment = { -1, -3 }

        };

        //Act
        var result = (ObjectResult)controller.UpdateEquipment(newEntity).Result;

        //Assert - Response
        result.StatusCode.ShouldBe(403);

    }

    [Fact]
    public void AddNonExistentEquipment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-2");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto
        {
            Id = -1,
            UserId = -2,
            Equipment = {-20 }

        };

        //Act
        var result = (ObjectResult)controller.UpdateEquipment(newEntity).Result;

        //Assert - Response
        result.StatusCode.ShouldBe(404);

    }

    private static bool IsEquipmentEqual(List<Equipment> equipmentList, List<int> idList)
    {
        if (equipmentList == null || idList == null) return false;

        if (equipmentList.Count != idList.Count) return false;

        var equipmentIds = new List<int>();

        foreach (var equipment in equipmentList)
        {
            equipmentIds.Add((int)equipment.Id);
        }

        equipmentIds.Sort();
        idList.Sort();

        return equipmentIds.SequenceEqual(idList);
    }

    private static TourController CreateController(IServiceScope scope, string number)
    {
        return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>())
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

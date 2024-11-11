using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Tours.Tests.Integration.Tour;

[Collection("Sequential")]
public class TourTests : BaseToursIntegrationTest
{
    public TourTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void UpdateEquipment_successful_updates_tour_equipment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto
        {
            Id = -1,
            UserId = -1,
            Equipment = {-1, -3},
            Name = "Kusla",
            Description = "Opis",
            Difficulty = 0,
            Tag = 0,
            Status = 0,
            Price = 0

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
    public void UpdateEquipment_successful_remove_equiment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-2");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var equipment = dbContext.Equipment.FirstOrDefault(i => i.Id == -2);
        var tour = dbContext.Tours.Include(t => t.Equipment).FirstOrDefault(t => t.Id == -3);


        tour.Equipment.Add(equipment);
        dbContext.SaveChanges();

        var newEntity = new TourDto
        {
            Id = -3,
            UserId = -2,
            Equipment = { }

        };

        //Act
        var result = (ObjectResult)controller.UpdateEquipment(newEntity).Result;

        //Assert - Response
        result.StatusCode.ShouldBe(200);

        //Assert - Database
        var storedEntity = dbContext.Tours
            .Where(i => i.Id == newEntity.Id)
            .ToList()
            .FirstOrDefault(i => IsEquipmentEqual(i.Equipment, newEntity.Equipment));

        storedEntity.ShouldNotBeNull();
        storedEntity.Equipment.Count.ShouldBe(0);

    }

    [Fact]
    public void Retrieves_all()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var result = ((ObjectResult)controller.GetAllByUserId().Result)?.Value as PagedResult<TourDto>;


        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(4);  
        result.TotalCount.ShouldBe(4);
    }

    //[Fact]
    //public void AddTour_successful_adds_tour()
    //{
    //    using var scope = Factory.Services.CreateScope();
    //    var controller = CreateController(scope, "-1");
    //    var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
    //    var newEntity = new TourDto
    //    {
    //        UserId = -1,
    //        Equipment = { -1 },
    //        Name = "Gala",
    //        Description = "Opis",
    //        Difficulty = TourDifficulty.Hard,
    //        Tag = TourTag.Adventure,
    //        Status = 0,
    //        Price = 0
    //    };
    //    var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourDto;

    //    result.ShouldNotBeNull();
    //    result.Name.ShouldBe(newEntity.Name);
    //    result.Description.ShouldBe(newEntity.Description);
    //    result.Difficulty.ShouldBe(newEntity.Difficulty);
    //    result.Status.ShouldBe(TourStatus.Draft);
    //    result.Price.ShouldBe(0);

    //    var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == newEntity.Name);
    //    storedEntity.ShouldNotBeNull();
    //}

    [Fact]
    public void AddTourWithCheckpoints_successful()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var tourWithCheckpoints = new TourWithCheckpointsDto
        {
            Tour = new TourDto
            {
                UserId = -1,
                Name = "AddTourWithCheckpoints",
                Description = "Opis",
                Difficulty = TourDifficulty.Hard,
                Tag = TourTag.Adventure,
                Status = 0,
                Price = 0
            },
            Checkpoints = new List<CheckpointDto>
                {
                new CheckpointDto
                {
                    Latitude = 1,
                    Longitude = 1,
                    Name = "Checkpoint1",
                    Description = "Description1",
                    Image = new TourImageDto { Data = "PRVASLIKAZATESTIRANJE", UploadedAt = DateTime.UtcNow, MimeType = "image/jpeg" }
                },
                new CheckpointDto
                {
                    Latitude = 2,
                    Longitude = 2,
                    Name = "Checkpoint2",
                    Description = "Description2",
                    Image = new TourImageDto { Data = "DRUGASLIKAZATESTIRANJE", UploadedAt = DateTime.UtcNow, MimeType = "image/jpeg" }
                },
                new CheckpointDto
                {
                    Latitude = 3,
                    Longitude = 3,
                    Name = "Checkpoint3",
                    Description = "Description3",
                    Image = new TourImageDto { Data = "TRECASLIKAZATESTIRANJE", UploadedAt = DateTime.UtcNow, MimeType = "image/jpeg" }
                }
            }
        };

        // Act
        var result = (ObjectResult)controller.Add(tourWithCheckpoints).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        //Assert - Database
        var storedTour = dbContext.Tours.FirstOrDefault(i => i.Name == tourWithCheckpoints.Tour.Name);
        storedTour.ShouldNotBeNull();
        storedTour.Checkpoints.Count.ShouldBe(3);
    }


    [Fact]
    public void UpdateEquipment_unsuccessful_unauthorized_user()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-2");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto
        {
            Id = -1,
            UserId = -1,
            Equipment = { -1, -3 },
            Name = "Gala",
            Description = "Opis",
            Difficulty = 0,
            Tag = 0,
            Status = 0,
            Price = 0

        };

        //Act
        var result = (ObjectResult)controller.UpdateEquipment(newEntity).Result;

        //Assert - Response
        result.StatusCode.ShouldBe(403);

    }

    [Fact]
    public void UpdateEquipment_unsuccessful_equipment_not_exist()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-2");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto
        {
            Id = -1,
            UserId = -2,
            Equipment = {-20 },
            Name = "Miha",
            Description = "Opis",
            Difficulty = 0,
            Tag = 0,
            Status = 0,
            Price = 0

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
        return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>(), scope.ServiceProvider.GetRequiredService<IEquipmentService>(), scope.ServiceProvider.GetRequiredService<ICheckpointService>())
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("personId", number),
                        new Claim("id", number)
                    }))
                }
            }
        };
    }

}

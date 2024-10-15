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

namespace Explorer.Tours.Tests.Integration.ClubInvitations;

[Collection("Sequential")]
public class ClubInvitationsTests: BaseToursIntegrationTest
{
    public ClubInvitationsTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void InviteToClub_successful_invite()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new ClubInviteDTO
        {
            OwnerId = -1,
            TouristId = -1,
            ClubId = -1,
            UserId = -1,
            Date = new DateTime(),
            Status = TourInviteStatus.Pending,
        };
        var result = ((ObjectResult)controller.InviteTourist(newEntity).Result)?.Value as ClubInviteDTO;

        //assert response
        result.ShouldNotBeNull();
        result.OwnerId.ShouldBe(newEntity.OwnerId);
        result.TouristId.ShouldBe(newEntity.TouristId);
        result.ClubId.ShouldBe(newEntity.ClubId);
        result.Status.ShouldBe(TourInviteStatus.Pending);
        //assert db
        var storedentity = dbContext.ClubInvites.
            FirstOrDefault(t => t.ClubId == newEntity.ClubId
            && t.TouristId == t.TouristId
            && t.OwnerId == t.OwnerId);

        storedentity.ShouldNotBeNull();
    }

    [Fact]
    public void InviteToClub_unsuccessful_invite()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new ClubInviteDTO
        {
            OwnerId = -1,
            TouristId = -1,
            ClubId = -1,
            UserId = -1,
            Date = new DateTime(),
            Status = TourInviteStatus.Pending,
        };
        var result = (ObjectResult)controller.InviteTourist(newEntity).Result;

        result.StatusCode.ShouldBe(409);
        //assert db
        var storedentity = dbContext.ClubInvites.
            Where(t => t.ClubId == newEntity.ClubId
            && t.TouristId == t.TouristId
            && t.OwnerId == t.OwnerId).FirstOrDefault();

        storedentity.ShouldNotBeNull();
    }

    [Fact]
    public void RemoveFromClub_successful_removal()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new ClubInviteDTO
        {
            OwnerId = -2,
            TouristId = -2,
            ClubId = -1,
            UserId = -1,
            Date = new DateTime(),
            Status = TourInviteStatus.Pending,
        };
        var result = (ObjectResult)controller.RemoveTourist(newEntity).Result;

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);
        //assert db
        var storedentity = dbContext.ClubInvites.
            FirstOrDefault(t => t.ClubId == newEntity.ClubId
            && t.TouristId == t.TouristId
            && t.OwnerId == t.OwnerId);

        storedentity.ShouldBeNull();
    }
    [Fact]
    public void RemoveFromClub_unsuccessful_removal()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-1");
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new ClubInviteDTO
        {
            OwnerId = -200,
            TouristId = 25,
            ClubId = -19999,
            UserId = -1,
            Date = new DateTime(),
            Status = TourInviteStatus.Pending,
        };
        var result = (ObjectResult)controller.RemoveTourist(newEntity).Result;

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(409);
        //assert db
        var storedentity = dbContext.ClubInvites.
            FirstOrDefault(t => t.ClubId == newEntity.ClubId
            && t.TouristId == t.TouristId
            && t.OwnerId == t.OwnerId);

        storedentity.ShouldBeNull();
    }

    private static ClubInviteController CreateController(IServiceScope scope, string number)
    {
        return new ClubInviteController(scope.ServiceProvider.GetRequiredService<IClubInviteService>())
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


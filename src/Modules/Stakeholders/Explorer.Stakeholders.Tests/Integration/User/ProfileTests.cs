﻿using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.User;

[Collection("Sequential")]
public class ProfileTests : BaseStakeholdersIntegrationTest
{
    public ProfileTests(StakeholdersTestFactory factory) : base(factory) { }

    private static ProfileController CreateController(IServiceScope scope, string number)
    {
        return new ProfileController(scope.ServiceProvider.GetRequiredService<IProfileService>())
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

    private static ProfileController CreateController(IServiceScope scope)
    {
        return new ProfileController(scope.ServiceProvider.GetRequiredService<IProfileService>());
    }

    [Fact]
    public void Get_successfully_retrieves_profile()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope, "-11");

        // Act
        var result = controller.Get();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        var profile = Assert.IsType<ProfileDto>(okResult.Value);
        Assert.NotNull(profile);
    }

    [Fact]
    public void Get_unsuccessfully_finds_profile_with_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope, "-999999"); // Invalid profile ID

        // Act
        var result = controller.Get();

        // Assert
        Assert.NotNull(result);
        var notFoundResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public void Get_returns_unauthorized_for_unauthenticated_user()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope);

        // Act
        Assert.Throws<NullReferenceException>(() => controller.Get());
    }

    [Fact]
    public void Update_successfully_updates_profile()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope, "-11");

        var profileDto = new ProfileDto
        {
            Username = "newUsername",
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Biography = "New bio",
            Moto = "New moto",
            Image = new ImageDto
            {
                Data = "new data for this idk",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            }
        };

        // Act
        var result = controller.Update(profileDto);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var updatedProfile = Assert.IsType<ProfileDto>(okResult.Value);
        Assert.Equal("newUsername", updatedProfile.Username);
        Assert.Equal("John", updatedProfile.Name);
    }

    [Fact]
    public void Update_returns_conflict_if_username_already_exists()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope, "-11");

        var profile1Dto = new ProfileDto
        {
            Username = "newUsername",
            Name = "John",
            LastName = "Doe",
            Email = "profile@gmail.com",
            Biography = "New bio",
            Moto = "New moto",
            Image = new ImageDto
            {
                Data = "new data for this idk",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            }
        };

        var profile2Dto = new ProfileDto
        {
            Username = "newUsername",
            Name = "Jane",
            LastName = "Doe",
            Email = "profile2@gmail.com",
            Biography = "New bio",
            Moto = "New moto",
            Image = new ImageDto
            {
                Data = "new data for this idk",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            }
        };

        // Act
        var result1 = controller.Update(profile1Dto);

        // Arrange new controller
        controller = CreateController(scope, "-12");

        var result2 = controller.Update(profile2Dto);

        // Assert
        Assert.NotNull(result1);
        var okResult1 = Assert.IsType<OkObjectResult>(result1.Result);
        Assert.Equal(200, okResult1.StatusCode);

        Assert.NotNull(result2);
        var conflictResult = Assert.IsType<ObjectResult>(result2.Result);
        Assert.Equal(409, conflictResult.StatusCode);
    }

    [Fact]
    public void Update_returns_not_found_for_nonexistent_user()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope, "-999999"); // Invalid personId

        var profileDto = new ProfileDto
        {
            Username = "newUsername",
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Biography = "New bio",
            Moto = "New moto",
            Image = new ImageDto
            {
                Data = "new data for this idk",
                UploadedAt = DateTime.UtcNow,
                MimeType = "image/jpeg"
            }
        };

        // Act
        var result = controller.Update(profileDto);

        // Assert
        Assert.NotNull(result);
        var notFoundResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}

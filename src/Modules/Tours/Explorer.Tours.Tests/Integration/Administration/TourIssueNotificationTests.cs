using Explorer.API.Controllers.User;
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
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourIssueNotificationTests : BaseToursIntegrationTest
    {
        public TourIssueNotificationTests(ToursTestFactory factory) : base(factory) { }

        private static TourIssueNotificationController CreateNotificationController(IServiceScope scope, string userId)
        {
            return new TourIssueNotificationController(
                scope.ServiceProvider.GetRequiredService<ITourIssueNotificationService>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim("userId", userId)
                        }))
                    }
                }
            };
        }

        [Fact]
        public async Task CreateNotification_Successfully()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateNotificationController(scope, "-1");

            var newNotification = new TourIssueNotificationDto
            {
                FromUserId = -1,
                ToUserId = -2,
                Status = BuildingBlocks.Core.Domain.Enums.TourIssueNotificationStatus.Unread,
                TourIssueReportId = -11
            };

            var result = ((ObjectResult) controller.Create(newNotification).Result)?.Value as TourIssueNotificationDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.FromUserId.ShouldBe(newNotification.FromUserId);
            result.ToUserId.ShouldBe(newNotification.ToUserId);
            result.Status.ShouldBe(newNotification.Status);
            result.TourIssueReportId.ShouldBe(newNotification.TourIssueReportId);

            // Assert - Database
            var storedNotification = dbContext.TourIssueNotifications.FirstOrDefault(n => n.Id == result.Id);
            storedNotification.ShouldNotBeNull();
            storedNotification.FromUserId.ShouldBe(newNotification.FromUserId);
            storedNotification.ToUserId.ShouldBe(newNotification.ToUserId);
            storedNotification.Status.ShouldBe(newNotification.Status);
            storedNotification.TourIssueReportId.ShouldBe(newNotification.TourIssueReportId);
        }

        [Fact]
        public async Task CreateNotification_Unsuccessfully_TourReport_Does_Not_Exist()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateNotificationController(scope, "-1");

            var newNotification = new TourIssueNotificationDto
            {
                FromUserId = -1,
                ToUserId = -2,
                Status = BuildingBlocks.Core.Domain.Enums.TourIssueNotificationStatus.Unread,
                TourIssueReportId = -9999 // Non-existent report ID
            };
            var result = (ObjectResult)controller.Create(newNotification)?.Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(409); // Expecting 409 Conflict due to invalid TourIssueReportId
        }

        [Fact]
        public async Task GetPagedNotifications_Successfully_Returns_All_Notifications()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateNotificationController(scope, "-1");

            // Act
            var result = ((ObjectResult)controller.GetForUserId(-1).Result)?.Value as PagedResult<TourIssueNotificationDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
            result.TotalCount.ShouldBe(3);
        }
    }
}

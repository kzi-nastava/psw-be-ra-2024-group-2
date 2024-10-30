using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourIssueReportTests : BaseToursIntegrationTest
    {
        public TourIssueReportTests(ToursTestFactory factory) : base(factory) { }

        private static TourIssueReportController CreateController(IServiceScope scope, string userId)
        {
            return new TourIssueReportController(scope.ServiceProvider.GetRequiredService<ITourIssueReportService>(), scope.ServiceProvider.GetRequiredService<ITourService>(), scope.ServiceProvider.GetRequiredService<ITourIssueCommentService>())
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
        private static TourIssueReportReviewController CreateControllerReview(IServiceScope scope, string number)
        {
            return new TourIssueReportReviewController(scope.ServiceProvider.GetRequiredService<ITourIssueReportService>(), scope.ServiceProvider.GetRequiredService<ITourService>())
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


        [Fact]
        public void Admin_can_view_all_tour_issue_reports()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateControllerReview(scope, "-1");

            // Act
            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<TourIssueReportDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(4);
            result.TotalCount.ShouldBe(4);
        }

        [Fact]
        public void CreateReport_successfully_saves_issue_report()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateController(scope, "-21");

            var newReport = new TourIssueReportDto
            {
                Id = -1,
                Category = "Safety",
                Description = "There was a broken railing on the path.",
                Priority = "High",
                TourId = -1,  // ID postojeće ture
                CreatedAt = DateTime.UtcNow,
                FixUntil = DateTime.UtcNow,
                Status = 0,
                UserId = -21
            };

            // Act
            var result = ((ObjectResult)controller.Create(newReport).Result)?.Value as TourIssueReportDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.TourId.ShouldBe(newReport.TourId);
            result.UserId.ShouldBe(-21); // Simulirani userId za turista

            // Assert - Database
            var storedReport = dbContext.TourIssueReports.FirstOrDefault(r => r.UserId == result.UserId && r.TourId == result.TourId);
            storedReport.ShouldNotBeNull();
            storedReport.TourId.ShouldBe(newReport.TourId);
            storedReport.UserId.ShouldBe(-21);
        }

        [Fact]
        public async void CreateReport_should_return_not_found_when_tour_does_not_exist()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-21");

            var newReport = new TourIssueReportDto
            {
                Id = -2,
                Category = "Safety",
                Description = "There was a broken railing on the path.",
                Priority = "High",
                TourId = -9999,
                CreatedAt = DateTime.UtcNow,
                FixUntil = DateTime.UtcNow,
                Status = 0,
                UserId = -21
            };

            // Act
            var result = (ObjectResult)controller.Create(newReport).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404); // Očekujte 404 Not Found
        }

        [Fact]
        public void CloseReport_Successfully()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateControllerReview(scope, "-1");

            var newReport = new TourIssueReportDto
            {
                Id = -11,
                Category = "Safety",
                Description = "There was a broken railing on the path.",
                Priority = "High",
                TourId = -2,
                CreatedAt = DateTime.UtcNow,
                FixUntil = DateTime.UtcNow,
                Status = TourIssueReportStatus.Open,
                UserId = -11
            };

            // Act
            var result = ((ObjectResult)controller.CloseReport(newReport).Result)?.Value as TourIssueReportDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(newReport.Id);
            result.UserId.ShouldBe(newReport.UserId);
            result.Status.ShouldBe(TourIssueReportStatus.Closed);

            //Assert - Database
            var storedReport = dbContext.TourIssueReports.FirstOrDefault(r => r.Id == result.Id);
            storedReport.ShouldNotBeNull();
            storedReport.Id.ShouldBe(newReport.Id);
            storedReport.UserId.ShouldBe(newReport.UserId);
            storedReport.Status.ShouldBe(TourIssueReportStatus.Closed);
        }

        [Fact]
        public void CloseReport_Unsuccessfully_Report_Does_Not_Exist()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateControllerReview(scope, "-1");

            var newReport = new TourIssueReportDto
            {
                Id = -9998,
                Category = "Safety",
                Description = "There was a broken railing on the path.",
                Priority = "High",
                TourId = -2,
                CreatedAt = DateTime.UtcNow,
                FixUntil = DateTime.UtcNow,
                Status = TourIssueReportStatus.Open,
                UserId = -11
            };

            // Act
            var result = ((ObjectResult)controller.CloseReport(newReport).Result)?.Value as TourIssueReportDto;

            // Assert - Response
            result.ShouldBeNull();
        }

        [Fact]
        public void SetFixUntilDate_Successfully()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateControllerReview(scope, "-1");

            var newReport = new TourIssueReportDto
            {
                Id = -11,
                Category = "Safety",
                Description = "There was a broken railing on the path.",
                Priority = "High",
                TourId = -2,
                CreatedAt = DateTime.UtcNow,
                FixUntil = DateTime.Parse("2025-11-15 14:30:00").ToUniversalTime(),
                Status = TourIssueReportStatus.Open,
                UserId = -11
            };

            // Act
            var result = ((ObjectResult)controller.SetFixUntilDate(newReport, -1).Result)?.Value as TourIssueReportDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(newReport.Id);
            result.UserId.ShouldBe(newReport.UserId);
            result.FixUntil.ShouldBe(newReport.FixUntil);

            //Assert - Database
            var storedReport = dbContext.TourIssueReports.FirstOrDefault(r => r.Id == result.Id);
            storedReport.ShouldNotBeNull();
            storedReport.Id.ShouldBe(newReport.Id);
            storedReport.UserId.ShouldBe(newReport.UserId);
            storedReport.FixUntil.ShouldBe(newReport.FixUntil);

            var storedTour = dbContext.Tours.Where(t => t.Id == storedReport.TourId).FirstOrDefault();

            var storedNotification = dbContext.TourIssueNotifications.
                FirstOrDefault(r => r.FromUserId == -1 && 
                               r.ToUserId == storedTour.UserId &&
                               r.Status == TourIssueNotificationStatus.Unread &&
                               r.TourIssueReportId == result.Id);
            storedNotification.ShouldNotBeNull();
            storedNotification.FromUserId.ShouldBe(-1);
            storedNotification.ToUserId.ShouldBe(storedTour.UserId);
            storedNotification.Status.ShouldBe(TourIssueNotificationStatus.Unread);
            storedNotification.TourIssueReportId.ShouldBe(result.Id);
        }

    }
}

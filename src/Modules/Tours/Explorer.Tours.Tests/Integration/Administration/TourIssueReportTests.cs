using Explorer.API.Controllers.Administrator.Administration;
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
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        public void CloseReport_Successfully_Closes_Report()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateControllerReview(scope, "-1");

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
            var result = (ObjectResult)controller.CloseReport(newReport).Result;

            // Assert
            //result.ShouldNotBeNull();
            //result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void MarkAsDone_successfully_mark_report_as_closed()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateController(scope, "-19");

            var existingReport = new TourIssueReportDto
            {
                Id = -12,
                Category = "Safety",
                Description = "Broken railing.",
                Priority = "High",
                TourId = -3,
                CreatedAt = DateTime.UtcNow,
                FixUntil = DateTime.UtcNow.AddDays(1),
                Status = TourIssueReportStatus.Closed, 
                UserId = -19
            };

            // Act
            var result = ((ObjectResult)controller.MarkAsDone(existingReport).Result)?.Value as TourIssueReportDto;

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(TourIssueReportStatus.Closed);

            var storedReport = dbContext.TourIssueReports.FirstOrDefault(r => r.Id == existingReport.Id);
            storedReport.ShouldNotBeNull();
            storedReport.Status.ShouldBe(TourIssueReportStatus.Closed);
        }

        [Fact]
        public void MarkAsDone_should_return_conflict_when_report_not_exist()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-21");

            var nonExistingReport = new TourIssueReportDto
            {
                Id = -9999, 
                TourId = -1,
                Category = "Safety",
                Description = "Non-existent report.",
                Status = 0,
                UserId = -21
            };

            // Act
            var result = (ObjectResult)controller.MarkAsDone(nonExistingReport).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(409); 
        }

        [Fact]
        public void AlertNotDone_successfully_update_fixUntil_date()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateController(scope, "-20");

            var existingReport = new TourIssueReportDto
            {
                Id = -13,
                Category = "Safety",
                Description = "Trail obstruction.",
                Priority = "High",
                TourId = -4,
                CreatedAt = DateTime.UtcNow,
                FixUntil = DateTime.UtcNow.AddDays(2),
                Status = TourIssueReportStatus.Open, 
                UserId = -20
            };

            // Act
            var result = ((ObjectResult)controller.AlertNotDone(existingReport).Result)?.Value as TourIssueReportDto;

            // Assert
            result.ShouldNotBeNull();
            result.FixUntil.ShouldBeGreaterThan(DateTime.UtcNow.AddDays(2).AddSeconds(-3));

            var storedReport = dbContext.TourIssueReports.FirstOrDefault(r => r.Id == existingReport.Id);
            storedReport.ShouldNotBeNull();
            storedReport.FixUntil.ShouldBeGreaterThan(DateTime.UtcNow.AddDays(2).AddSeconds(-3));
        }

        [Fact]
        public void AlertNotDone_should_return_conflict_for_invalid_status_transition()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-21");

            var conflictingReport = new TourIssueReportDto
            {
                Id = -13,
                TourId = -1,
                Category = "Safety",
                Description = "Conflicting update.",
                FixUntil = DateTime.UtcNow,
                Status = 0,
                UserId = -21
            };

            // Act
            var result = (ObjectResult)controller.AlertNotDone(conflictingReport).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(409); 
        }

    }
}

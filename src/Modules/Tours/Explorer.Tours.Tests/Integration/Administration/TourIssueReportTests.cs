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

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourIssueReportTests : BaseToursIntegrationTest
    {
        public TourIssueReportTests(ToursTestFactory factory) : base(factory) { }

        private static TourIssueReportController CreateController(IServiceScope scope, string userId)
        {
            return new TourIssueReportController(scope.ServiceProvider.GetRequiredService<ITourIssueReportService>())
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
        public void CreateReport_successfully_saves_issue_report()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateController(scope, "-21");

            var newReport = new TourIssueReportDto
            {
                Category = "Safety",
                Description = "There was a broken railing on the path.",
                Priority = "High",
                TourId = -22,  // ID postojeće ture
                DateTime = DateTime.UtcNow,
                UserId = -21
            };

            // Act
            var result = ((ObjectResult)controller.Create(newReport).Result)?.Value as TourIssueReportDto;

            // Assert - Response
            result.ShouldNotBeNull();
             // Proveravamo da je kreiran ID
            result.TourId.ShouldBe(newReport.TourId);
            result.UserId.ShouldBe(123); // Simulirani userId za turista

            // Assert - Database
            var storedReport = dbContext.TourIssueReports.FirstOrDefault(r => r.UserId == result.UserId && r.TourId == result.TourId);
            storedReport.ShouldNotBeNull();
            storedReport.TourId.ShouldBe(newReport.TourId);
            storedReport.UserId.ShouldBe(123);
        }


    }
}

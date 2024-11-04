using Explorer.API.Controllers.Tourist;
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
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourIssueCommentTests : BaseToursIntegrationTest
    {
        public TourIssueCommentTests(ToursTestFactory factory) : base(factory) { }

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
        private static TourIssueCommentController CreateCommentController(IServiceScope scope, string userId)
        {
            return new TourIssueCommentController(scope.ServiceProvider.GetRequiredService<ITourIssueCommentService>(), scope.ServiceProvider.GetRequiredService<ITourIssueReportService>())
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
        public void CreateComment_Successfully()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateCommentController(scope, "-1");

            var newTourIssueComment = new TourIssueCommentDto
            {
                Id = -1,
                UserId = -1,
                TourIssueReportId = -11,
                Comment = "Neki komentar",
                PublishedAt = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.CreateComment(-1,newTourIssueComment).Result)?.Value as TourIssueCommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.TourIssueReportId.ShouldBe(newTourIssueComment.TourIssueReportId);
            result.UserId.ShouldBe(-1);

            // Assert - Database
            var storedReport = dbContext.TourIssueComments.FirstOrDefault(r => r.Id == result.Id);
            storedReport.ShouldNotBeNull();
            storedReport.TourIssueReportId.ShouldBe(newTourIssueComment.TourIssueReportId);
            storedReport.Comment.ShouldBe(newTourIssueComment.Comment);
            storedReport.PublishedAt.ShouldBe(newTourIssueComment.PublishedAt);
            storedReport.UserId.ShouldBe(-1);
        }

        [Fact]
        public async void CreateComment_Unsuccessfully_TourReport_Does_Not_Exist()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateCommentController(scope, "-1");

            var newTourIssueComment = new TourIssueCommentDto
            {
                Id = -6548,
                UserId = -1,
                TourIssueReportId = -9799,
                Comment = "Neki komentar",
                PublishedAt = DateTime.UtcNow
            };

            // Act
            var result = (ObjectResult)controller.CreateComment(-1,newTourIssueComment).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(409); // Očekujte 404 Not Found
        }

        [Fact]
        public void GetPaged_Successfully_Return_All_Comments()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateCommentController(scope, "-1");


            // Act
            var result = ((ObjectResult)controller.GetAllComments(-11, 0, 0).Result)?.Value as PagedResult<TourIssueCommentDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
            result.TotalCount.ShouldBe(3);
        }


        [Fact]
        public void GetPaged_Unsuccessfully_Tour_Does_Not_Exist()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateCommentController(scope, "-1");


            // Act
            var result = (ObjectResult)controller.GetAllComments(-9999, 0, 0).Result;

            // Assert
            result.StatusCode.ShouldBe(404);
        }

    }
}

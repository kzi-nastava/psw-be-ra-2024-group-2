using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
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

namespace Explorer.Tours.Tests.Integration.Tour
{
    [Collection("Sequential")]
    public class TourReviewTests : BaseToursIntegrationTest
    {
        public TourReviewTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Create_successful_add_review()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new TourReviewDto
            {
                UserId = -1,
                TourId = -1,
                Grade = 1,
                Comment = "string",
                Image = new TourImageDto
                {
                    Data = "new dataas gala",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                ReviewDate = DateTime.MinValue,
                VisitDate = DateTime.MinValue,
                Progress = 0

            };

            //act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourReviewDto;

            //assert-response 
            result.UserId.ShouldBe(newEntity.UserId);
            result.TourId.ShouldBe(newEntity.TourId);

            //assert-database
            var storedEntity = dbContext.TourReview
            .FirstOrDefault(i => i.UserId == newEntity.UserId && i.TourId == newEntity.TourId);

            storedEntity.ShouldNotBeNull();
            storedEntity.UserId.ShouldBe(result.UserId);
            storedEntity.TourId.ShouldBe(result.TourId);

        }

        [Fact]
        public void Create_unsuccessful_tour_not_exist()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new TourReviewDto
            {
                UserId = -1,
                TourId = 10,
                Grade = 1,
                Comment = "string",
                Image = new TourImageDto
                {
                    Data = "druga slika",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                ReviewDate = DateTime.MinValue,
                VisitDate = DateTime.MinValue,
                Progress = 0,

            };

            //act
            var result = (ObjectResult)controller.Create(newEntity).Result;

            //assert-response
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Create_Unsuccessful_invalid_grade()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new TourReviewDto
            {
                UserId = -1,
                TourId = -1,
                Grade = 10,
                Comment = "string",
                Image = new TourImageDto
                {
                    Data = "druga slika",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                ReviewDate = DateTime.MinValue,
                VisitDate = DateTime.MinValue,
                Progress= 0,

            };
           
            //act
            var result = (ObjectResult)controller.Create(newEntity).Result;

            //assert-response
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void GetPagedByTourId_successfull()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "1");

            // Act
            var result = ((ObjectResult)controller.GetPagedByTourId(-10).Result)?.Value as PagedResult<TourReviewDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(2);
            result.TotalCount.ShouldBe(2);
        }
        [Fact]
        public void Update_successful()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var updatedReviewDto = new TourReviewDto
            {
                UserId = -1,
                TourId = -1,
                Grade = 5,
                Comment = "Updated Comment",
                Image = new TourImageDto
                {
                    Data = "updated data",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/png"
                },
                ReviewDate = DateTime.UtcNow,
                VisitDate = DateTime.UtcNow.AddDays(-5),
                Progress = 100
            };

            //Act
            var result = ((ObjectResult)controller.UpdateReview(updatedReviewDto).Result)?.Value as TourReviewDto;

            //Assert - Response
            //result.Id.ShouldBe(-1);

            //Assert - Database

            result.ShouldNotBeNull();
            //storedEntity.Id.ShouldBe(result.Id);
        }
        [Fact]
        public void Update_successful_comment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var updatedReviewDto = new TourReviewDto
            {
                UserId = -1,
                TourId = -1,
                Grade = 5,
                Comment = "Updated Comment another version",
                Image = new TourImageDto
                {
                    Data = "updated dataaaaaa",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/png"
                },
                ReviewDate = DateTime.UtcNow,
                VisitDate = DateTime.UtcNow.AddDays(-5),
                Progress = 100
            };

            //Act
            var result = ((ObjectResult)controller.UpdateReview(updatedReviewDto).Result)?.Value as TourReviewDto;

            //Assert - Response
            //result.Id.ShouldBe(-1);

            //Assert - Database

            result.ShouldNotBeNull();
            //storedEntity.Id.ShouldBe(result.Id);
        }
        [Fact]
        public void Update_successful_grade()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var updatedReviewDto = new TourReviewDto
            {
                UserId = -1,
                TourId = -1,
                Grade = 4,
                Comment = "Updated Comment another version",
                Image = new TourImageDto
                {
                    Data = "updated dataaaaaa",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/png"
                },
                ReviewDate = DateTime.UtcNow,
                VisitDate = DateTime.UtcNow.AddDays(-5),
                Progress = 100
            };

            //Act
            var result = ((ObjectResult)controller.UpdateReview(updatedReviewDto).Result)?.Value as TourReviewDto;

            //Assert - Response
            //result.Id.ShouldBe(-1);

            //Assert - Database

            result.Grade.ShouldBeInRange(1,5);
            result.Grade.ShouldBePositive();
            //storedEntity.Id.ShouldBe(result.Id);
        }
        [Fact]
        public void Update_Unsuccessful_Negative_Grade()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var updatedReviewDto = new TourReviewDto
            {
                UserId = -1,
                TourId = -1,
                Grade = -2, // Invalid negative grade
                Comment = "Invalid grade test",
                Image = new TourImageDto
                {
                    Data = "some image data",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                ReviewDate = DateTime.UtcNow,
                VisitDate = DateTime.UtcNow.AddDays(-5),
                Progress = 100
            };

            // Act
            var result = (ObjectResult)controller.UpdateReview(updatedReviewDto).Result;

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400); // 400 indicates a bad request due to invalid input
            result.Value.ShouldBeOfType<ProblemDetails>(); // Adjusted to check for ProblemDetails

            var problemDetails = result.Value as ProblemDetails;
            problemDetails.ShouldNotBeNull();
        }
        [Fact]
        public void Update_Unsuccessful_Large_Grade()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var updatedReviewDto = new TourReviewDto
            {
                UserId = -1,
                TourId = -1,
                Grade = 50, // Invalid negative grade
                Comment = "Invalid grade test",
                Image = new TourImageDto
                {
                    Data = "some immmage data",
                    UploadedAt = DateTime.UtcNow,
                    MimeType = "image/jpeg"
                },
                ReviewDate = DateTime.UtcNow,
                VisitDate = DateTime.UtcNow.AddDays(-5),
                Progress = 100
            };

            // Act
            var result = (ObjectResult)controller.UpdateReview(updatedReviewDto).Result;

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400); // 400 indicates a bad request due to invalid input
            result.Value.ShouldBeOfType<ProblemDetails>(); // Adjusted to check for ProblemDetails

            var problemDetails = result.Value as ProblemDetails;
            problemDetails.ShouldNotBeNull();
        }

        private static TourReviewController CreateController(IServiceScope scope, string number)
        {
            return new TourReviewController(scope.ServiceProvider.GetRequiredService<ITourReviewService>(), scope.ServiceProvider.GetRequiredService<ITourService>())
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
}

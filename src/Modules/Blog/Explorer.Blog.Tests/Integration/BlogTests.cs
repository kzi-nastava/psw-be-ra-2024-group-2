using Explorer.API.Controllers;
using Explorer.API.Controllers.Author;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Infrastructure.Database;
using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Explorer.Blog.Tests.Integration
{
    [Collection("Sequential")]
    public class BlogTests : BaseBlogIntegrationTest
    {
        public BlogTests(BlogTestFactory factory) : base(factory)
        {
        }
        /*
        [Fact]
        public void Creates_blog()
        {
            try
            {
                // Arrange
                using var scope = Factory.Services.CreateScope();
                var controller = CreateController(scope);
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
                var newBlog = new BlogDto
                {
                    Title = "Test Blog Title",
                    Description = "This is a test blog description.",
                    Status = Status.Published,
                    AuthorId = 1,
                    Ratings = new List<RatingDto?> {
                        new RatingDto
                        {
                            RatingType = "Upvote",
                            CreatedAt = DateTime.UtcNow,
                            Username = "mika123"
                        },
                        new RatingDto
                        {
                            RatingType = "Downvote",
                            CreatedAt = DateTime.UtcNow,
                            Username = "zika321"
                        }
                    },
                    Images = new List<Image?>
                    {
                        new Image
                        {
                            Data = "ImageData1",
                            UploadedAt = DateTime.UtcNow,
                            MimeType = MimeType.Jpeg
                        },
                        new Image
                        {
                            Data = "ImageData2",
                            UploadedAt = DateTime.UtcNow,
                            MimeType = MimeType.Png
                        }
                    }
                };

                // Act
                var result = ((ObjectResult)controller.Create(newBlog).Result)?.Value as BlogDto;

                // Assert - Response
                result.ShouldNotBeNull();
                result.Title.ShouldBe(newBlog.Title);
                result.Description.ShouldBe(newBlog.Description);
                result.Status.ShouldBe(newBlog.Status);
                result.AuthorId.ShouldBe(newBlog.AuthorId);
                result.Images.ShouldNotBeNull();
                result.Images.Count.ShouldBe(2);

                // Assert - Database
                var storedBlog = dbContext.Blogs.FirstOrDefault(b => b.Id == result.Id);
                storedBlog.ShouldNotBeNull();
                storedBlog.Description.ShouldBe(result.Description);
                result.Status.ToString().ShouldBe(newBlog.Status.ToString());
                storedBlog.AuthorId.ShouldBe(result.AuthorId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }
        */
        [Fact]
        public void Fails_to_create_blog_with_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var invalidBlog = new BlogDto
            {
                Title = "",
                Description = "This blog should not be created because it has invalid data.",
                Status = Status.Draft,
                AuthorId = 0,
                Images = new List<Image?>
        {
            new Image
            {
                Data = "InvalidImageData",
                UploadedAt = DateTime.UtcNow,
                MimeType = MimeType.Jpeg
            }
        }
            };

            // Act
            var result = ((ObjectResult)controller.Create(invalidBlog).Result)?.Value as BlogDto;

            // Assert - Response should be null or a failure response
            result.ShouldBeNull();
        }

        private static BlogController CreateController(IServiceScope scope, string userId)
        {
            return new BlogController(scope.ServiceProvider.GetRequiredService<IBlogService>())
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
    }

}
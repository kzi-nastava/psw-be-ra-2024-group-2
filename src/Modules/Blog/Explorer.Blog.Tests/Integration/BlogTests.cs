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

namespace Explorer.Blog.Tests.Integration
{
    public class BlogTests : BaseBlogIntegrationTest
    {
        public BlogTests(BlogTestFactory factory) : base(factory)
        {
        }

        public void Creates_blog()
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
            var storedBlog = dbContext.Blogs.FirstOrDefault(b => b.Title == newBlog.Title);
            storedBlog.ShouldNotBeNull();
            storedBlog.Description.ShouldBe(result.Description);
            result.Status.ToString().ShouldBe(newBlog.Status.ToString());
            storedBlog.AuthorId.ShouldBe(result.AuthorId);
        }

        [Fact]
        public void Fails_to_create_blog_with_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Example of invalid blog data (missing title and authorId)
            var invalidBlog = new BlogDto
            {
                Title = "", // Invalid: Empty title
                Description = "This blog should not be created because it has invalid data.",
                Status = Status.Draft,
                AuthorId = 0, // Invalid: AuthorId cannot be 0
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

        private static BlogController CreateController(IServiceScope scope)
        {
            return new BlogController(scope.ServiceProvider.GetRequiredService<IBlogService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}

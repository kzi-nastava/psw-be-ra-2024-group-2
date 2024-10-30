using Explorer.API.Controllers;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration;

[Collection("Sequential")]
public class CommentCommandTests : BaseBlogIntegrationTest
{
    public CommentCommandTests(BlogTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates_comment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var newComment = new CommentDTO
        {
            BlogId = 1,
            Text = "Ovo je test komentar.",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = ((ObjectResult)controller.Create(newComment.BlogId,newComment).Result)?.Value as CommentDTO;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.BlogId.ShouldBe(newComment.BlogId);
        result.Text.ShouldBe(newComment.Text);

        // Assert - Database
        var storedComment = dbContext.Comments.FirstOrDefault(c => c.Text == newComment.Text);
        storedComment.ShouldNotBeNull();
        storedComment.BlogId.ShouldBe(result.BlogId);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var invalidComment = new CommentDTO
        {
            BlogId = 0, // Nevalidan BlogId
            Text = ""   // Nevalidan tekst
        };

        // Act
        var result = (ObjectResult)controller.Create(invalidComment.BlogId,invalidComment).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400); // Očekuj HTTP 400 Bad Request
    }

    [Fact]
    public void Updates_comment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var initialComment = new CommentDTO
        {
            BlogId = 1,
            Text = "Originalni komentar."
        };

        // Kreiraj inicijalni komentar
        var createdComment = ((ObjectResult)controller.Create(initialComment.BlogId,initialComment).Result)?.Value as CommentDTO;

        // Ažuriraj komentar
        var updatedComment = new CommentDTO
        {
            Id = createdComment.Id,
            BlogId = 1,
            Text = "Ažuriran komentar."
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedComment.Id, updatedComment).Result)?.Value as CommentDTO;

        // Assert - Response
        result.ShouldNotBeNull();
        result.BlogId.ShouldBe(updatedComment.BlogId);
        result.Text.ShouldBe(updatedComment.Text);

        // Assert - Database
        var storedComment = dbContext.Comments.FirstOrDefault(c => c.Id == updatedComment.Id);
        storedComment.ShouldNotBeNull();
        storedComment.Text.ShouldBe(updatedComment.Text);
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new CommentDTO
        {
            Text = "Ažurirani komentar." // Ne postavljamo ID jer koristimo nevalidan
        };

        // Act & Assert
        var exception = Should.Throw<KeyNotFoundException>(() => controller.Update(-1000, updatedEntity));
        exception.Message.ShouldBe("404: Not found: -1000");
    }



    [Fact]
    public void Deletes_comment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

        // Kreiraj komentar koji ćeš obrisati
        var commentToDelete = new CommentDTO
        {
            BlogId = 1,
            Text = "Komentar za brisanje."
        };
        var createdComment = ((ObjectResult)controller.Create(commentToDelete.BlogId, commentToDelete).Result)?.Value as CommentDTO;

        // Act
        var result = (OkResult)controller.Delete(createdComment.Id);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedComment = dbContext.Comments.FirstOrDefault(c => c.Id == createdComment.Id);
        storedComment.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act & Assert
        var exception = Should.Throw<KeyNotFoundException>(() => controller.Delete(-1000));
        exception.Message.ShouldBe("404: Not found: -1000");
    }


    private static CommentController CreateController(IServiceScope scope)
    {
        return new CommentController(scope.ServiceProvider.GetRequiredService<ICommentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}

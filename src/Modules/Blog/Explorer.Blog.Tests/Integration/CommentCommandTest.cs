using Explorer.API.Controllers;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Blog.Core.Domain;

namespace Explorer.Blog.Tests.Integration;

[Collection("Sequential")]
public class CommentCommandTests : BaseBlogIntegrationTest
{
    public CommentCommandTests(BlogTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates_Comment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

        // Očistimo stare zapise u tabeli (opciono, zavisno od konfiguracije baze)
        dbContext.Blogs.RemoveRange(dbContext.Blogs);
        dbContext.SaveChanges();

        // Kreiramo blog
        var blog = new Explorer.Blog.Core.Domain.Blog(
            title: "Test Blog",
            description: "Ovo je test opis bloga.",
            status: Explorer.Blog.Core.Domain.Status.Published,
            authorId: 1
        );
        dbContext.Blogs.Add(blog);
        dbContext.SaveChanges();

        // Dohvatamo generisani BlogId
        var blogId = blog.Id;

        // Kreiramo komentar povezan sa kreiranim blogom
        var newComment = new CommentDTO
        {
            BlogId = blogId, // koristimo dobijeni BlogId
            Text = "Ovo je test komentar.",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = ((ObjectResult)controller.Create(newComment.BlogId, newComment).Result)?.Value as CommentDTO;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.BlogId.ShouldBe(newComment.BlogId);
        result.Text.ShouldBe(newComment.Text);

        // Assert - Database
        var storedComment = dbContext.Comments.FirstOrDefault(c => c.Text == newComment.Text);
        storedComment.ShouldNotBeNull();
        storedComment.BlogId.ShouldBe(blogId); // Provera BlogId-a
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
        var result = (ObjectResult)controller.Create(invalidComment.BlogId, invalidComment).Result;

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

        // Kreirajte novi blog na koji će komentar biti povezan
        var blog = new Explorer.Blog.Core.Domain.Blog(
            title: "Test Blog",
            description: "Opis za test blog.",
            status: Explorer.Blog.Core.Domain.Status.Published,
            authorId: 1
        );
        dbContext.Blogs.Add(blog);
        dbContext.SaveChanges();

        // Kreirajte inicijalni komentar povezan sa kreiranim blogom
        var initialComment = new CommentDTO
        {
            BlogId = blog.Id, // postavljamo na ID kreiranog bloga
            Text = "Originalni komentar."
        };
        var createdComment = ((ObjectResult)controller.Create(initialComment.BlogId, initialComment).Result)?.Value as CommentDTO;

        // Ažurirajte komentar sa novim tekstom
        var updatedComment = new CommentDTO
        {
            Id = createdComment.Id,
            BlogId = createdComment.BlogId, // koristimo isti BlogId kao inicijalni komentar
            Text = "Ažuriran komentar.",
            CreatedAt = createdComment.CreatedAt
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedComment.Id, updatedComment.BlogId, updatedComment).Result)?.Value as CommentDTO;

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
        var exception = Should.Throw<KeyNotFoundException>(() => controller.Update(-1000, updatedEntity.BlogId, updatedEntity));
        exception.Message.ShouldBe("404: Not found: -1000");
    }



    [Fact]
    public void Deletes_comment()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

        // Kreirajte novi blog kako bi se komentar mogao pravilno povezati
        var blog = new Explorer.Blog.Core.Domain.Blog(
            title: "Test Blog za brisanje",
            description: "Opis bloga za brisanje komentara.",
            status: Explorer.Blog.Core.Domain.Status.Published,
            authorId: 1
        );
        dbContext.Blogs.Add(blog);
        dbContext.SaveChanges();

        // Kreiraj komentar koji će biti obrisan, povezan sa kreiranim blogom
        var commentToDelete = new CommentDTO
        {
            BlogId = blog.Id, // postavljamo ID kreiranog bloga
            Text = "Komentar za brisanje."
        };
        var createdComment = ((ObjectResult)controller.Create(commentToDelete.BlogId, commentToDelete).Result)?.Value as CommentDTO;

        // Act
        var result = (OkResult)controller.Delete(createdComment.Id, createdComment.BlogId);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedComment = dbContext.Comments.FirstOrDefault(c => c.Id == createdComment.Id);
        storedComment.ShouldBeNull(); // Komentar treba da bude obrisan iz baze
    }


    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act & Assert
        var exception = Should.Throw<KeyNotFoundException>(() => controller.Delete(-1000, 1));
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
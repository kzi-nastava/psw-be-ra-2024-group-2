using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Blog.API.Dtos; // Uveri se da imaš pravilne uvoze
using Explorer.Blog.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.API.Controllers;
using Explorer.Blog.API.Public;

namespace Explorer.Blog.Tests.Integration.Administration;

[Collection("Sequential")]
public class CommentQueryTests : BaseBlogIntegrationTest
{
    public CommentQueryTests(BlogTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

    }

    private static CommentController CreateController(IServiceScope scope)
    {
        return new CommentController(scope.ServiceProvider.GetRequiredService<ICommentService>())
        {
            ControllerContext = BuildContext("-1") // Uveri se da je ovaj deo ispravan za tvoj kontekst
        };
    }
}

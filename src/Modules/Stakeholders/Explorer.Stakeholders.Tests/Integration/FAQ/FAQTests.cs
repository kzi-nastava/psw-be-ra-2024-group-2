using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.User;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
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

namespace Explorer.Stakeholders.Tests.Integration.FAQ
{
    [Collection("Sequential")]
    public class FAQTests : BaseStakeholdersIntegrationTest
    {
        public FAQTests(StakeholdersTestFactory factory) : base(factory) { }

        private static FAQCreateController CreateController(IServiceScope scope, string number)
        {
            return new FAQCreateController(scope.ServiceProvider.GetRequiredService<IFAQService>())
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
        private static FAQViewController ViewController(IServiceScope scope, string number)
        {
            return new FAQViewController(scope.ServiceProvider.GetRequiredService<IFAQService>())
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
        public void Admin_can_create_faq()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var newFaq = new FAQDto
            {
                Question = "What is the cancellation policy?",
                Answer = "You can cancel up to 24 hours before the tour for a full refund.",
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Create(-1, newFaq).Result)?.Value as FAQDto;

            // Assert
            result.ShouldNotBeNull();
            result.Question.ShouldBe(newFaq.Question);
            result.Answer.ShouldBe(newFaq.Answer);
            result.CreatedDate.ShouldNotBe(default);
        }

        [Fact]
        public void Non_admin_cannot_create_faq()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-2");
            var newFaq = new FAQDto
            {
                Question = "What are the payment options?",
                Answer = "We accept credit cards and PayPal.",
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Create(-2, newFaq).Result);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(409);
        }

        [Fact]
        public void Anyone_can_view_all_faqs()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = ViewController(scope, "-3"); 
            var page = 1;
            var pageSize = 10;

            // Act
            var result = ((ObjectResult)controller.GetAll(page, pageSize).Result)?.Value as PagedResult<FAQDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(1);
            result.TotalCount.ShouldBe(1);
        }

        [Fact]
        public void Create_faq_fails_when_question_is_missing()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var newFaq = new FAQDto
            {
                Question = null,
                Answer = "We accept credit cards and PayPal.",
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Create(-1, newFaq).Result);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(409);
        }

        [Fact]
        public void Create_faq_fails_when_answer_is_whitespace()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1");
            var newFaq = new FAQDto
            {
                Question = "What is the cancellation policy?",
                Answer = " ",
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Create(-1, newFaq).Result);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(409);
        }
    }
}

using Explorer.API.Controllers.Author;
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

namespace Explorer.Tours.Tests.Integration.Tour
{
    [Collection("Sequential")]

    public class CheckpointTests : BaseToursIntegrationTest
    {
        public CheckpointTests(ToursTestFactory factory) : base(factory)
        {}
   
        private static CheckpointController CreateController(IServiceScope scope, string number)
        {
            return new CheckpointController(scope.ServiceProvider.GetRequiredService<ICheckpointService>())
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

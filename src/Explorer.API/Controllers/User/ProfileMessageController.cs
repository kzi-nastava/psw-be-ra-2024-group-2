using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "allLoggedPolicy")]
    [Route("api/profile/messaging")]
    public class ProfileMessageController : BaseApiController
    {
        private readonly IProfileMessageService _profileMessageService;
        public ProfileMessageController(IProfileMessageService profileMessageService)
        {
            _profileMessageService = profileMessageService;
        }

        [HttpPost("new/message")]
        public ActionResult<ProfileMessageDto> Create([FromBody] ProfileMessageDto profileMessage)
        {
            var result = _profileMessageService.Create(profileMessage, User.PersonId());
            return CreateResponse(result);
        }
    }
}

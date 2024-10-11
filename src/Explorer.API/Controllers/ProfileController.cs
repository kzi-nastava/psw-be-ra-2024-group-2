using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers
{
    [Authorize(Policy = "touristOrAuthorPolicy")]
    [Route("api/profile")]
    public class ProfileController : BaseApiController
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public ActionResult<ProfileDto> Get()
        {
            var profile = _profileService.Get(User.PersonId());
            return CreateResponse(profile);
        }

        [HttpPut]
        public ActionResult<ProfileDto> Update([FromBody] ProfileDto profile)
        {
            var updatedProfile = _profileService.Update(User.PersonId(), profile);
            return CreateResponse(updatedProfile);
        }
    }
}



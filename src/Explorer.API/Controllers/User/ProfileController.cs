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

        [HttpGet("all")]
        public ActionResult<IEnumerable<ProfileDto>> GetAllProfiles()
        {
            var profiles = _profileService.GetAllUsers();
            return Ok(profiles);
        }

        [HttpGet("{id:long}")]
        public ActionResult<ProfileDto> GetById(long id)
        {
            var profile = _profileService.Get(id);
            return CreateResponse(profile);
        }

        [HttpGet("all-raw")]
        public ActionResult<List<ProfileDto>> GetAllProfilesRaw()
        {
            var result = _profileService.GetAllUsers();

            if (result.IsFailed) return BadRequest(result.Reasons);

            return result.Value;
        }

    }
}
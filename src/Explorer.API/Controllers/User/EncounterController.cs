using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "allLoggedPolicy")] // Replace with your appropriate policy
    [Route("api/encounters")]
    public class EncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;

        public EncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        // Get Encounter by Name
        [HttpGet("{name}")]
        public ActionResult<EncounterDto> GetByName(string name)
        {
            var result = _encounterService.GetByName(name);
            return CreateResponse(result);
        }

        // Get Encounter by Id
        [HttpGet("id/{id}")]
        public ActionResult<EncounterDto> GetById(long id)
        {
            var result = _encounterService.GetById(id);
            return CreateResponse(result);
        }

        // Get All Encounters
        [HttpGet]
        public ActionResult GetAll()
        {
            var result = _encounterService.GetAll();
            return CreateResponse(result);
        }

        // Create Social Encounter
        [HttpPost("social")]
        public ActionResult<SocialEncounterDto> CreateSocialEncounter([FromBody] SocialEncounterDto encounterDto)
        {
            var result = _encounterService.CreateSocialEncounter(encounterDto);
            return CreateResponse(result);
        }


        // Create Hidden Location Encounter
        [HttpPost("hidden-location")]
        public ActionResult<HiddenLocationEncounterDto> CreateHiddenLocationEncounter([FromBody] HiddenLocationEncounterDto encounterDto)
        {
            var result = _encounterService.CreateHiddenLocationEncounter(encounterDto);
            return CreateResponse(result);
        }

        // Create Misc Encounter
        [HttpPost("misc")]
        public ActionResult<MiscEncounterDto> CreateMiscEncounter([FromBody] MiscEncounterDto encounterDto)
        {
            var result = _encounterService.CreateMiscEncounter(encounterDto);
            return CreateResponse(result);
        }

        // Update Social Encounter
        [HttpPut("social")]
        public ActionResult<SocialEncounterDto> UpdateSocialEncounter([FromBody] SocialEncounterDto encounterDto)
        {
            var result = _encounterService.UpdateSocialEncounter(encounterDto);
            return CreateResponse(result);
        }

        // Update Hidden Location Encounter
        [HttpPut("hidden-location")]
        public ActionResult<HiddenLocationEncounterDto> UpdateHiddenLocationEncounter([FromBody] HiddenLocationEncounterDto encounterDto)
        {
            var result = _encounterService.UpdateHiddenLocationEncounter(encounterDto);
            return CreateResponse(result);
        }

        // Update Misc Encounter
        [HttpPut("misc")]
        public ActionResult<MiscEncounterDto> UpdateMiscEncounter([FromBody] UnifiedEncounterDto encounterDto)
        {
            var result = _encounterService.UpdateMiscEncounter(encounterDto);
            return CreateResponse(result);
        }

        // Delete Encounter
        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            var result = _encounterService.Delete(id);
            return CreateResponse(result);
        }
    }
}

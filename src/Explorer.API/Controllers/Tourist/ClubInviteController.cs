using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/clubInvite")]
    public class ClubInviteController: BaseApiController
    {
        private readonly IClubInviteService _clubInviteService;
        public ClubInviteController(IClubInviteService tourService)
        {
            _clubInviteService = tourService;
        }

        [HttpPut("invite")]
        public ActionResult<ClubInviteDTO> InviteTourist([FromBody] ClubInviteDTO dto)
        {
            var result = _clubInviteService.Invite(dto);
            return CreateResponse(result);
        }


        [HttpPut("remove")]
        public ActionResult<ClubInviteDTO> RemoveTourist([FromBody] ClubInviteDTO dto)
        {
            var result = _clubInviteService.Remove(dto);
            return CreateResponse(result);
        }
    }
}
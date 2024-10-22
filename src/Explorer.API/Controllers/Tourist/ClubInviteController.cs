using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Stakeholders.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/clubInvite")]
    public class ClubInviteController: BaseApiController
    {
        private readonly IClubInviteService _clubInviteService;
        private readonly IAccountService _accountService;
        public ClubInviteController(IClubInviteService tourService, IAccountService accountService)
        {
            _clubInviteService = tourService;
            _accountService = accountService;
        }

        [HttpPut("invite")]
        public ActionResult<ClubInviteDTO> InviteTourist([FromBody] ClubInviteDTO dto)
        {
            var result = _clubInviteService.Invite(dto);
            return CreateResponse(result);
        }

        [HttpDelete("remove")]
        public ActionResult<ClubInviteDTO> RemoveTourist([FromBody] ClubInviteDTO dto)
        {
            var result = _clubInviteService.Remove(dto);
            return CreateResponse(result);
        }
        [HttpGet("getClubInvites")]
        public ActionResult<PagedResult<ClubInviteDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _clubInviteService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
        [HttpGet("getTourists")]
        public ActionResult<PagedResult<AccountDto>> GetAllTourists([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _accountService.GetPagedTourists(page, pageSize);
            return CreateResponse(result);
        }
    }
}
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    //[Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/club")]
    public class ClubController : BaseApiController
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpPost]
        public ActionResult<ClubDto> Create([FromBody] ClubDto club)
        {
            var userId = User.PersonId();
            var result = _clubService.Create(userId, club);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ClubDto> Update([FromRoute] int id, [FromBody] ClubDto club)
        {
            
            var result = _clubService.Update(id, club);
            return CreateResponse(result);
        }

    }
}

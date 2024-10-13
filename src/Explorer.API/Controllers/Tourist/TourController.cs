using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy ="authorPolicy")]
    [Route("api/author/tour")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;
        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpPut("equipment")]
        public ActionResult<TourDto> UpdateEquipment([FromBody] TourDto tour)
        {
            var result = _tourService.UpdateTour(tour, User.PersonId());
            return CreateResponse(result);
        }
    }
}

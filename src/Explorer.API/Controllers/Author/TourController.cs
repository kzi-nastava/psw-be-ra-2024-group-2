using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
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

        [HttpGet]
        public ActionResult<PagedResult<TourDto>> GetAllByUserId()
        {
            var allTours = _tourService.GetAllByUserId(User.UserId());
            return CreateResponse(allTours.ToResult());
        }


        [HttpPost]
        public ActionResult<TourDto> Create([FromBody] TourDto dto)
        {

            var addedTour = _tourService.CreateTour(dto,User.UserId());
            return CreateResponse(addedTour);
        }
       
    }
}

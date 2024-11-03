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
        private readonly IEquipmentService _equipmentService;
        private readonly ICheckpointService _checkpointService;
        public TourController(ITourService tourService, IEquipmentService equipmentService, ICheckpointService checkpointService)
        {
            _tourService = tourService;
            _equipmentService = equipmentService;
            _checkpointService = checkpointService;
        }

        [HttpPut("checkpoints")]
        public ActionResult<TourDto> UpdateCheckpoints([FromBody] TourDto tour)
        {
            var result = _tourService.UpdateTourCheckpoints(tour, User.PersonId());
            return CreateResponse(result);
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

        //[HttpPost]
        //public ActionResult<TourDto> Create([FromBody] TourDto dto)
        //{

        //   // var addedTour = _tourService.CreateTour(dto,User.UserId());
        //    return CreateResponse(addedTour);
        //}

        [HttpPost("addNew")]
        public ActionResult<TourDto> AddNew([FromBody] TourWithCheckpointsDto dto)
        {
            var addedTour = _tourService.CreateTour(dto.Tour, dto.Checkpoints,User.UserId());
            return CreateResponse(addedTour);
        }

        [HttpGet("{tourId}")]
        public ActionResult<TourDto> GetById(long tourId)
        {
            var tour = _tourService.GetById(tourId);
            return CreateResponse(tour);
        }

        [HttpGet("equipment/getAll")]
        public ActionResult<PagedResult<EquipmentDto>> GetAll()
        {
            var result = _equipmentService.GetPaged(1, int.MaxValue);
            return CreateResponse(result);
        }
    }
}

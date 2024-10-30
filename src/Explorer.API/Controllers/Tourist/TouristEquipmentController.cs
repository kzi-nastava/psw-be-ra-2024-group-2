using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/equipment")]
    public class TouristEquipmentController : BaseApiController
    {
        private readonly IEquipmentService _equipmentService;

        public TouristEquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpPost("add/{equipmentId:long}")]
        public ActionResult AddEquipmentToTourist(long equipmentId)
        {
            var result = _equipmentService.AddEquipmentTourist(User.PersonId(), equipmentId);
            return CreateResponse(result);
        }

        [HttpDelete("remove/{equipmentId:long}")]
        public ActionResult RemoveEquipmentFromTourist(long equipmentId)
        {
            var result = _equipmentService.RemoveEquipmentFromTourist(User.PersonId(), equipmentId);
            return CreateResponse(result);
        }

        [HttpGet]
        public ActionResult GetTouristEquipment()
        {
            var result = _equipmentService.GetEquipmentForTourist(User.PersonId());
            return Ok(result);
        }

        [HttpGet("all")]
        public ActionResult<PagedResult<EquipmentDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _equipmentService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
    }
}

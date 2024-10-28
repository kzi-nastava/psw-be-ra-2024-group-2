using Explorer.BuildingBlocks.Core.UseCases;
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

        [HttpPost("{touristId:int}/add/{equipmentId:long}")]
        public ActionResult AddEquipmentToTourist(long touristId, long equipmentId)
        {
            var result = _equipmentService.AddEquipmentTourist(touristId, equipmentId);
            return CreateResponse(result);
        }

        [HttpDelete("{touristId:long}/remove/{equipmentId:long}")]
        public ActionResult RemoveEquipmentFromTourist(long touristId, long equipmentId)
        {
            var result = _equipmentService.RemoveEquipmentFromTourist(touristId, equipmentId);
            return CreateResponse(result);
        }

        [HttpGet("{touristId:long}")]
        public ActionResult GetTouristEquipment(long touristId)
        {
            var result = _equipmentService.GetEquipmentForTourist(touristId);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<PagedResult<EquipmentDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _equipmentService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }



    }
}

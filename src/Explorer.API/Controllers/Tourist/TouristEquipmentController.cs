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
    }
}

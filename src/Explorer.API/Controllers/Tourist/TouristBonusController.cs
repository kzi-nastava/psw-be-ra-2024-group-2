using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/touristBonus")]
    public class TouristBonusController : BaseApiController
    {
        private readonly ITouristBonusService _touristBonusService;
        public TouristBonusController(ITouristBonusService touristBonusService)
        {
            _touristBonusService = touristBonusService;
        }

        [HttpPost("create/{touristId}/{discountPercentage}")]
        public ActionResult<TouristBonusDto> CreateTouristBonus(long touristId, int discountPercentage)
        {
            var touristBonus = _touristBonusService.Create(touristId, discountPercentage);
            return CreateResponse(touristBonus);
        }

        [HttpGet("{touristId}")]
        public ActionResult<TouristBonusDto> GetById(long touristId)
        {
            var touristBonus = _touristBonusService.GetById(touristId);
            return CreateResponse(touristBonus);
        }

        [HttpPut("use/{touristId}/{couponCode}")]
        public ActionResult<TouristBonusDto> UseTouristBonus(long touristId, string couponCode)
        {
            var touristBonus = _touristBonusService.Use(touristId, couponCode);
            return CreateResponse(touristBonus);
        }
    }
}

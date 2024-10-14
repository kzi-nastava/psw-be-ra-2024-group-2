using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/object")]
    public class ObjectController : BaseApiController
    {
        private readonly IObjectService _objectService;
        public ObjectController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        [HttpPost("objectCreation")]
        public ActionResult<ObjectDto> CreateObject([FromBody] ObjectDto tourObject)
        {
            var result = _objectService.CreateObject(tourObject);
            return CreateResponse(result);
        }
    }
}

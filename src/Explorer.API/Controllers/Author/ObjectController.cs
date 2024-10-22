using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/tourObject")]
    public class ObjectController : BaseApiController
    {
        private readonly IObjectService _objectService;
        public ObjectController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        [HttpGet]
        public ActionResult<PagedResult<ObjectDto>> GetAll()
        {
            var allObjects = _objectService.GetAll();
            return allObjects;
        }

        [HttpPost]
        public ActionResult<ObjectDto> CreateObject([FromBody] ObjectDto tourObject)
        {
            var result = _objectService.Create(tourObject);
            return CreateResponse(result);
        }


        [HttpPut("{id}")]
        public ActionResult<ObjectDto> UpdateObject(int id, [FromBody] double[] coordinates)
        {
            var result = _objectService.UpdateObject(id, coordinates);
            return CreateResponse(result);
        }
    }
}

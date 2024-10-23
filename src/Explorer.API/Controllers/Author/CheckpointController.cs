using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/checkpoint")]
    public class CheckpointController : BaseApiController
    {
        private readonly ICheckpointService _checkpointService;

        public CheckpointController(ICheckpointService checkpointService)
        {
            _checkpointService = checkpointService;
        }

        [HttpPost]
        public ActionResult<CheckpointDto> Create([FromBody] CheckpointDto checkpoint) {
            var result = _checkpointService.Create(checkpoint);
            return CreateResponse(result);
        }

        [HttpGet("checkpoints/getAll")]
        public ActionResult<PagedResult<CheckpointDto>> GetAll()
        {
            var result = _checkpointService.GetPaged(1, int.MaxValue);
            return CreateResponse(result);
        }
    }
}

using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
        //[Authorize(Policy = "authorPolicy")]
        [Route("api/author/event")]
        public class EventController : BaseApiController
        {
            private readonly IEventService _eventService;
            public EventController(IEventService eventService)
            {
            _eventService = eventService;
            }

            [HttpGet]
            public ActionResult<PagedResult<EventDto>> GetAll()
            {
                var allEvents= _eventService.GetAll();
                return allEvents;
            }

            [HttpPost]
            public ActionResult<EventDto> CreateEvent([FromBody] EventDto tourEvent)
            {
                var result = _eventService.Create(tourEvent);
                return CreateResponse(result);
            }
        }
    }

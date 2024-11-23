using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface IEventService
    {
        PagedResult<EventDto> GetAllEventsWithinRange(int v, double longitude, double latitude);
        Result<EventDto> Create(EventDto tourEvent);
        PagedResult<EventDto> GetAll();
        Result SubscribeToEvent(EventDto eventDto, int userId);
    }
}

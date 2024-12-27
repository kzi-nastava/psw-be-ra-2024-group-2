using Explorer.BuildingBlocks.Core.Domain.Enums;
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
        Result <EventDto> SubscribeToEvent(EventDto eventDto, int userId);
        public PagedResult<EventDto> GetSortedByAcceptance();
        public PagedResult<EventDto> GetTopThree();
        public PagedResult<TourDto> GetNearTours(long eventId);

        public Result Delete(long id);  

        PagedResult<EventDto> FindAllByCategories(List<EventCategory> categories);
        Result Update(EventDto tourEvent, int v);
    }
}

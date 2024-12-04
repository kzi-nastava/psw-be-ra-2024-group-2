using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.UseCases.Administration;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/eventsubscription")]
public class EventSubscriptionController : BaseApiController
{
    private readonly IEventSubscriptionService _eventSubscriptionService;
    private readonly IEventService _eventService;

    public EventSubscriptionController(IEventSubscriptionService eventSubscriptionService, IEventService eventService)
    {
        _eventSubscriptionService = eventSubscriptionService;
        _eventService = eventService;
    }

    [HttpPost]
    public PagedResult<EventDto> Add([FromBody] List<EventCategory> categories)
    {


        _eventSubscriptionService.SubscribeEvent(User.UserId(), categories);
        var result = _eventService.FindAllByCategories(categories);

        return result;
    }

    [HttpPost("load")]
    public PagedResult<EventDto> Load([FromBody] List<EventCategory> categories)
    {
        var result = _eventService.FindAllByCategories(categories);

        return result;
    }



    [HttpDelete]
    public Result Delete()
    {
       var result =  _eventSubscriptionService.UnsubscribeEvent(User.UserId());

        return result;
    }
    [HttpGet("getSubscriptions")]
    public PagedResult<EventCategory> GetSubscriptionsByUserId()
    {


        var result = _eventSubscriptionService.GetByUserId(User.UserId());


        return result;
    }


}


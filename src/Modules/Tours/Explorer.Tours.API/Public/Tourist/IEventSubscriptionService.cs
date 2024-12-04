using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public interface IEventSubscriptionService
    {
        PagedResult<EventCategory> GetByUserId(int v);
        Result SubscribeEvent(int v, List<EventCategory> categories);
        Result UnsubscribeEvent(int v);
    }
}
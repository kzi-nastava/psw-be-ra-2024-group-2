using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class EventSubscriptionService : CrudService<EventSubscriptionDto,EventSubscription>, IEventSubscriptionService
    {
    
        ICrudRepository<EventSubscription> _eventSubscriptionRepository;

        public EventSubscriptionService(ICrudRepository<EventSubscription> eventSubscriptionRepository, IMapper mapper) : base(eventSubscriptionRepository,mapper)
        {
            _eventSubscriptionRepository = eventSubscriptionRepository;
        }

        public PagedResult<EventCategory> GetByUserId(int v)
        {
            var eventSubscriptions = _eventSubscriptionRepository
                                     .GetPaged(1, int.MaxValue)
                                     .Results; 
            var filteredSubscriptions = eventSubscriptions
                .Where(e => e.UserId == v).Select(e => e.EventCategory)
            .ToList();


            return new PagedResult<EventCategory>(filteredSubscriptions, filteredSubscriptions.Count());
        }

        public Result SubscribeEvent(int userId, List<EventCategory> categories)
        {
            var eventSubscriptions = _eventSubscriptionRepository
                          .GetPaged(1, int.MaxValue)
                          .Results; 

            foreach (EventCategory category in categories)
            {
                bool alreadyExists = eventSubscriptions.Any(sub =>
                    sub.UserId == userId && sub.EventCategory == category);

                if (!alreadyExists)
                {
                    EventSubscriptionDto eventSubscriptionDto = new EventSubscriptionDto
                    {
                        EventCategory = category,
                        Status = 0,
                        UserId = userId
                    };

                    _eventSubscriptionRepository.Create(MapToDomain(eventSubscriptionDto));
                }
            }

            return Result.Ok();
        }

        public Result UnsubscribeEvent(int v)
        {
            var eventSubscriptions = _eventSubscriptionRepository
               .GetPaged(1, int.MaxValue)
               .Results; 

            var userSubscriptions = eventSubscriptions
                .Where(e => e.UserId == v)
                .ToList();

            foreach (var subscription in userSubscriptions)
            {
                _eventSubscriptionRepository.Delete(subscription.Id);
            }

            return Result.Ok();
        }
    }
}

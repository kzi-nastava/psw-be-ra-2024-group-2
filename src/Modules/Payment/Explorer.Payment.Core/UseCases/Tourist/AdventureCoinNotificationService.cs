using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.UseCases.Tourist
{
    public class AdventureCoinNotificationService : IAdventureCoinNotificationService
    {
        private readonly IAdventureCoinNotificationRepository _repository;

        public AdventureCoinNotificationService(IAdventureCoinNotificationRepository repository)
        {
            _repository = repository;
        }

        public List<ActivationCoinNotificationDto> GetNotificationsForTourist(long touristId)
        {
            var notifications = _repository.GetByTouristId(touristId);
            return notifications.Select(n => new ActivationCoinNotificationDto
            {
                Id = n.Id,
                TouristId = n.TouristId,
                Status = n.IsRead,
                SentAt = n.SentAt
            }).ToList();
        }

        
    }

}

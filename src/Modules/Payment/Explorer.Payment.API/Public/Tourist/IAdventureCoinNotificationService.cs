using Explorer.Payment.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Public.Tourist
{
    public interface IAdventureCoinNotificationService
    {
        List<ActivationCoinNotificationDto> GetNotificationsForTourist(long touristId);
        void MarkNotificationAsRead(long notificationId);
        void MarkAllNotificationsAsRead(long touristId);
    }

}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain.RepositoryInterfaces
{
    public interface IAdventureCoinNotificationRepository
    {
        List<AdventureCoinNotification> GetByTouristId(long touristId);
        AdventureCoinNotification GetById(long notificationId);
        void Add(AdventureCoinNotification notification);

        void Update(AdventureCoinNotification notification);

    }
}

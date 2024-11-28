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
        Task<List<AdventureCoinNotification>> GetByTouristId(long touristId);
        Task<AdventureCoinNotification?> GetById(long notificationId);
        Task Add(AdventureCoinNotification notification);
        Task Update(AdventureCoinNotification notification);

    }
}

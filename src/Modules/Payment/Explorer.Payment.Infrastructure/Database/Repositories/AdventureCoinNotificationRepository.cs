using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Infrastructure.Database.Repositories
{
    using Explorer.Payment.Core.Domain;
    using Explorer.Payment.Core.Domain.RepositoryInterfaces;
    using Microsoft.EntityFrameworkCore;

    public class AdventureCoinNotificationRepository : IAdventureCoinNotificationRepository
    {
        private readonly PaymentContext _context;

        public AdventureCoinNotificationRepository(PaymentContext context)
        {
            _context = context;
        }

        public List<AdventureCoinNotification> GetByTouristId(long touristId)
        {
            return _context.AdventureCoinNotifications
                .Where(n => n.TouristId == touristId)
                .ToList();
        }

        public AdventureCoinNotification GetById(long notificationId)
        {
            return _context.AdventureCoinNotifications
                .FirstOrDefault(n => n.Id == notificationId);
        }

        public void Add(AdventureCoinNotification notification)
        {
            _context.AdventureCoinNotifications.Add(notification);
            _context.SaveChanges();
        }


        public void  Update(AdventureCoinNotification notification)
        {
            _context.AdventureCoinNotifications.Update(notification);
             _context.SaveChanges();
        }
    }

}

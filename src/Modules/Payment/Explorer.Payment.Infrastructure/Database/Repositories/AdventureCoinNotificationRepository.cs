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

        public async Task<List<AdventureCoinNotification>> GetByTouristId(long touristId)
        {
            return await _context.AdventureCoinNotifications
                .Where(n => n.TouristId == touristId)
                .ToListAsync();
        }

        public async Task<AdventureCoinNotification?> GetById(long notificationId)
        {
            return await _context.AdventureCoinNotifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);
        }

        public async Task Add(AdventureCoinNotification notification)
        {
            await _context.AdventureCoinNotifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task Update(AdventureCoinNotification notification)
        {
            _context.AdventureCoinNotifications.Update(notification);
            await _context.SaveChangesAsync();
        }
    }

}

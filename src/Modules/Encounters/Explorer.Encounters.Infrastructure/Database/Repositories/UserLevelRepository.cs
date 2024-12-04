using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class UserLevelRepository : IUserLevelRepository
    {
        private readonly EncountersContext _context;

        public UserLevelRepository(EncountersContext context)
        {
            _context = context;
        }

        // Get a UserLevel by ID
        public UserLevel GetById(long id)
        {
            return _context.UserLevels.Find(id);
        }

        // Add a new UserLevel
        public void AddUserLevel(UserLevel userLevel)
        {
            _context.UserLevels.Add(userLevel);
            _context.SaveChanges();
        }

        // Update an existing UserLevel
        public void UpdateUserLevel(UserLevel userLevel)
        {
            var existingUserLevel = _context.UserLevels.Find(userLevel.Id);
            if (existingUserLevel != null)
            {
                _context.Entry(existingUserLevel).CurrentValues.SetValues(userLevel);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("User level not found.");
            }
        }

        // Get all UserLevels
        public List<UserLevel> GetAllUserLevels()
        {
            return _context.UserLevels.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Infrastructure.Database;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly StakeholdersContext _dbContext;

        public ClubRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Club Get(int id)
        {
            var club = _dbContext.Clubs.FirstOrDefault(c => c.Id == id);
            if (club == null) throw new KeyNotFoundException("Club not found.");
            return club;
        }
        public Club? GetByName(string name)
        {
            return _dbContext.Clubs.FirstOrDefault(c => c.Name == name);
        }
        public Club Create(Club club)
        {
            _dbContext.Clubs.Add(club);
            _dbContext.SaveChanges();
            return club;
        }
        public Club Update(Club club)
        {
            _dbContext.Clubs.Update(club);
            _dbContext.SaveChanges();
            return club;
        }
        public void Delete(int id)
        {
            var club = Get(id);
            if (club != null)
            {
                _dbContext.Clubs.Remove(club);
                _dbContext.SaveChanges();
            }
        }
    }
}

using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class ClubInviteRepository : CrudDatabaseRepository<ClubInvite, ToursContext>, IClubInviteRepository
    {
        public ClubInviteRepository(ToursContext dbContext) : base(dbContext)
        {
        }
        public bool Exists(long clubId, long touristId)
        {
            return DbContext.ClubInvites.Where(t => t.ClubId == clubId && t.TouristId == touristId).Count()>0;
        }
        public ClubInvite GetByClubTourist(long clubId, long touristId)
        {
            return DbContext.ClubInvites.Where(t => t.ClubId == clubId && t.TouristId == touristId).First();
        }
    }
}
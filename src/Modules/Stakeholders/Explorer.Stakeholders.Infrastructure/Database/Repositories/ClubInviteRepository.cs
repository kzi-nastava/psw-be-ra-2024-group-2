using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class ClubInviteRepository : CrudDatabaseRepository<ClubInvite, StakeholdersContext>, IClubInviteRepository
    {
        public ClubInviteRepository(StakeholdersContext dbContext) : base(dbContext)
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
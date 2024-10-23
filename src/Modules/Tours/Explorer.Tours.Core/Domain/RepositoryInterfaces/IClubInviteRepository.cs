using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface IClubInviteRepository
    {
        public ClubInvite Update(ClubInvite clubInvite);
        public void Delete(long id);
        bool Exists(long clubId, long touristId);
        public ClubInvite GetByClubTourist(long clubId, long touristId);
        PagedResult<ClubInvite> GetPaged(int page, int pageSize);
    }
}

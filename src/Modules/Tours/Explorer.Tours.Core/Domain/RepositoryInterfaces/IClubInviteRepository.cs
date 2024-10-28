using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface IClubInviteRepository
    {
        public ClubInvite Update(ClubInvite clubInvite);
        public void Delete(long id);
        bool Exists(long clubId, long touristId);
        bool Exists(long id);
        public ClubInvite GetByClubTourist(long clubId, long touristId);
        public ClubInvite Get(long id);
        PagedResult<ClubInvite> GetPaged(int page, int pageSize);
    }
}

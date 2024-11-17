using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class ClubInviteService: CrudService<ClubInviteDTO, ClubInvite>, IClubInviteService
    {
        private readonly ICrudRepository<ClubInvite> _crudClubInviteRepository;
        private readonly IClubInviteRepository _clubInviteRepository;
        public ClubInviteService(IClubInviteRepository clubInviteRepository,
            ICrudRepository<ClubInvite> repository,
            IMapper mapper) : base(repository, mapper)
        {
            _crudClubInviteRepository = repository;
            _clubInviteRepository = clubInviteRepository;
        }
        public Result<ClubInviteDTO> Invite(ClubInviteDTO dto)
        {
            if (_clubInviteRepository.Exists(dto.ClubId, dto.TouristId))
            {
                return Result.Fail(FailureCode.Conflict).WithError("Invite to this club already exists.");
            }
            ClubInvite invite = _crudClubInviteRepository.Create(MapToDomain(dto));
            //dodati u klub kada kolega napravi repo za klub
            return MapToDto(invite);
        }
        public Result<ClubInviteDTO> Remove(long clubId, long touristId)
        {
            // Check if the club invite exists for the given clubId and touristId
            if (!_clubInviteRepository.Exists(clubId, touristId))
            {
                return Result.Fail(FailureCode.Conflict).WithError("Invite to this club doesn't exist.");
            }

            // Retrieve the club invite using the clubId and touristId
            ClubInvite clubInvite = _clubInviteRepository.GetByClubTourist(clubId, touristId);

            // Ensure the clubInvite was found before attempting to delete
            if (clubInvite == null)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Club invite not found.");
            }

            // Delete the club invite
            _crudClubInviteRepository.Delete(clubInvite.Id);
            return Result.Ok();
        }

        public Result<PagedResult<ClubInviteDTO>> GetPaged(int page, int pageSize)
        {
            var accounts = _clubInviteRepository.GetPaged(page, pageSize);
            return MapToDto(accounts);
        }
    }
}
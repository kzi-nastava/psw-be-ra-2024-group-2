using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Public.Administration;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Result<ClubInviteDTO> Remove(ClubInviteDTO dto)
        {
            if (!_clubInviteRepository.Exists(dto.ClubId, dto.TouristId)) {
                return Result.Fail(FailureCode.Conflict).WithError("Invite to this club doesn't exist.");
            }
            ClubInvite clubInvite = _clubInviteRepository.GetByClubTourist(dto.ClubId, dto.TouristId);


            _crudClubInviteRepository.Delete(clubInvite.Id);
            return Result.Ok(dto);
        }


        public Result<PagedResult<ClubInviteDTO>> GetPaged(int page, int pageSize)
        {
            var accounts = _clubInviteRepository.GetPaged(page, pageSize);
            return MapToDto(accounts);
        }
    }
}
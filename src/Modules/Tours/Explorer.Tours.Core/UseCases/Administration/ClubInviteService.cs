using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
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
                return Result.Fail("Invite to this club already exists.");
            }
            ClubInvite invite = _crudClubInviteRepository.Create(MapToDomain(dto));
            return MapToDto(invite);
        }
        public Result<ClubInviteDTO> Remove(ClubInviteDTO dto)
        {
            if (!_clubInviteRepository.Exists(dto.ClubId, dto.TouristId)) {
                return Result.Fail("Tourist is not a member of this club.");
            }
            ClubInvite clubInvite = _clubInviteRepository.GetByClubTourist(dto.ClubId, dto.TouristId);


            _crudClubInviteRepository.Delete(clubInvite.Id);
            return Result.Ok(dto);
        }
    }
}
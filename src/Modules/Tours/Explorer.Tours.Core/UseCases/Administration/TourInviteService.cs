using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourInviteService: CrudService<TourInviteDTO, TourInvite>, ITourInviteService
    {
        private readonly ICrudRepository<TourInvite> _tourInviteRepository;
        public TourInviteService(ICrudRepository<TourInvite> repository, IMapper mapper) : base(repository, mapper)
        {
            _tourInviteRepository = repository;
        }
        public Result<TourInviteDTO> Invite(TourInviteDTO dto)
        {
            var pagedResult = _tourInviteRepository.GetPaged(1, int.MaxValue);
            List<TourInvite> inviteList = pagedResult.Results.ToList();
            if (inviteList.Where(t => t.OwnerId == dto.OwnerId && t.TouristId == dto.TouristId).ToList().Count > 0)
            {
                return Result.Fail("Invite to this club already exists.");
            }
            TourInvite invite = _tourInviteRepository.Create(MapToDomain(dto));
            return MapToDto(invite);
        }
        public Result<TourInviteDTO> Remove(TourInviteDTO dto)
        {
            var pagedResult = _tourInviteRepository.GetPaged(1, int.MaxValue);
            List<TourInvite> inviteList = pagedResult.Results.ToList();
            if (inviteList.Where(t => t.OwnerId == dto.OwnerId && t.TouristId == dto.TouristId).ToList().Count == 0)
            {
                return Result.Fail("This person is not a part of this club.");
            }
            _tourInviteRepository.Delete(dto.Id);
            return Result.Ok(dto);
        }
    }
}
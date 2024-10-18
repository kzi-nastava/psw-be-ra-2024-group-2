using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface IClubInviteService
    {
        Result<ClubInviteDTO> Invite(ClubInviteDTO dto);
        Result<ClubInviteDTO> Remove(ClubInviteDTO dto);
        Result<PagedResult<ClubInviteDTO>> GetPaged(int page, int pageSize);
    }
}

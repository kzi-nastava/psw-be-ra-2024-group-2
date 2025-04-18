﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface IClubService
    {
        Result<ClubDto> Create(int userId, ClubDto club);
        Result<ClubDto> Update(int id, ClubDto club);
        PagedResult<ClubDto> GetAll();

    }
}

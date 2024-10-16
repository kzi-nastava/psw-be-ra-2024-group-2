﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface IClubService
    {
        Result<ClubDto> Create(ClubDto club);
        Result<ClubDto> Update(ClubDto club);

    }
}

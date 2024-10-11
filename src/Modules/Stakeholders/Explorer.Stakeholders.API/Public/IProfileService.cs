﻿using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public;

public interface IProfileService
{
    Result<ProfileDto> Get(long personId);
    Result<ProfileDto> Update(long personId, ProfileDto profile);
}

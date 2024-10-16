using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IRatingApplicationService
    {
        Result<PagedResult<RatingApplicationDto>> GetPaged(int page, int pageSize);
        Result<RatingApplicationDto> Get(long id);
        Result<RatingApplicationDto> Create( long userId,RatingApplicationDto ratingApplication);
        Result<RatingApplicationDto> Update(long personId, RatingApplicationDto ratingApplication);
    }
}

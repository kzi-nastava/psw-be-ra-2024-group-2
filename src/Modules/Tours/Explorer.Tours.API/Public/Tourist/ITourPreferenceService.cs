using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface ITourPreferenceService
    {
        Result<TourPreferenceDto> Create(TourPreferenceDto equipment);
        PagedResult<TourPreferenceDto> GetByTouristId(long touristId);   
    }
}

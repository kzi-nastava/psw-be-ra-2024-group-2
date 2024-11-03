using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration;

public interface ITourService
{
    Result<TourDto> UpdateTour(TourDto tourDto, long userId);
    Result<TourDto> CreateTour(TourDto dto, int userId);
    PagedResult<TourDto> GetAllByUserId(int userId);
    Result<TourDto> GetById(long tourId);
    Result<PagedResult<TourDto>> GetPaged(int page, int pageSize);
    Result<TourDto> UpdateTourCheckpoints(TourDto tourDto, long userId);
    Result DeleteById(int tourId);
    PagedResult<TourDto> GetToursNearby(int v, LocationDto locationDto);
}

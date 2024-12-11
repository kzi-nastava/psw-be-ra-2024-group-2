using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ICheckpointService
    {
        Result<PagedResult<CheckpointDto>> GetPaged(int page, int pageSize);
        PagedResult <CheckpointDto> GetAllById(List<long> ids);
        Result<PagedResult<CheckpointDto>> GetAllByTourId(long tourId);
        List<TourExecutionCheckpointDto> CheckDistance(List<TourExecutionCheckpointDto> execCheckpoints, double lon, double lat);



    }
}

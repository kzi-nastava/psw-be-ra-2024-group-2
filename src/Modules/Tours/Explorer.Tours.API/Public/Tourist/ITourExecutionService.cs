using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface ITourExecutionService
    {
        Result<TourExecutionDto> Create(int tourId, int userId);
        Result<TourExecutionDto> EndTour(int v, TourExecutionDto dto);
        Result<TourExecutionDto> GetByUserId(int userId);
        Result<TourExecutionDto> Update(TourExecutionDto tourExecutionDto);

    }
}

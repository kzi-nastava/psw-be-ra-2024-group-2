using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface ITourExecutionCheckpointService
    {
        Result Create(int tourExecutionId, TourDto tourDto);

    }
}

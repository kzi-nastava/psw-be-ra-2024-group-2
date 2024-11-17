using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Internal;

public interface ITourPurchaseTokenService_Internal
{
    public Result<PagedResult<TourPurchaseTokenDto>> GetPaged(int page, int pageSize);
}

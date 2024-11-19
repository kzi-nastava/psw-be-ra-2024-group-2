using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Internal;
using Explorer.Payment.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.UseCases.Tourist;

public class TourPurchaseTokenService : CrudService<TourPurchaseTokenDto, TourPurchaseToken>, ITourPurchaseTokenService_Internal
{
    private readonly ICrudRepository<TourPurchaseToken> _tourPurchaseTokenRepository;
    private readonly IMapper mapper;

    public TourPurchaseTokenService(ICrudRepository<TourPurchaseToken> tourPurchaseTokenRepository, IMapper mapper) : base(tourPurchaseTokenRepository, mapper)
    {
        _tourPurchaseTokenRepository = tourPurchaseTokenRepository;
        this.mapper = mapper;
    }

    public Result<PagedResult<TourPurchaseTokenDto>> GetPaged(int page, int pageSize)
    {
        var pagedResult = base.GetPaged(page, pageSize);

        if (pagedResult.IsFailed)
        {
            return Result.Fail(pagedResult.Errors);
        }

        var filteredReviews = pagedResult.Value.Results;

        return Result.Ok(pagedResult.Value);
    }
}

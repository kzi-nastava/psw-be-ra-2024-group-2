using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Public.Author;

public interface ICouponAuthorService
{
    public Result<CouponDto> CreateCoupon(long authorId, long tourId, int discountPercentage, bool allToursDisount);
}

using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.Core.Domain;
using Explorer.Stakeholders.API.Internal;
using Explorer.Tours.API.Internal.Administration;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.UseCases.Author;

public class CouponAuthorService : CrudService<CouponDto, Coupon> , ICouponAuthorService
{
    private readonly ICrudRepository<Coupon> _couponRepository;

    public CouponAuthorService(ICrudRepository<Coupon> couponRepository, IMapper mapper) : base(couponRepository, mapper)
    {
        _couponRepository = couponRepository;
    }

    public Result<CouponDto> CreateCoupon(long authorId, long tourId, int discountPercentage, bool allToursDisount)
    {
        if(discountPercentage < 0 || discountPercentage > 100)
            return Result.Fail("Discount percentage must be between 0 and 100");
        if (allToursDisount == true)
        {
            tourId = -100;
        }

        Coupon coupon = new Coupon(authorId, tourId, discountPercentage, allToursDisount);

        _couponRepository.Create(coupon);

        return MapToDto(coupon);
    }
}

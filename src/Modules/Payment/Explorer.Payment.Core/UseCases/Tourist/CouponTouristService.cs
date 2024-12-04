using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.UseCases.Tourist;

public class CouponTouristService : CrudService<CouponDto, Coupon>, ICouponTouristService
{
    private readonly ICrudRepository<Coupon> _couponRepository;

    public CouponTouristService(ICrudRepository<Coupon> couponRepository, IMapper mapper) : base(couponRepository, mapper)
    {
        _couponRepository = couponRepository;
    }
    public Result<CouponDto> UseCoupon(string code)
    {

        Coupon? coupon = _couponRepository.GetPaged(1, int.MaxValue).Results.FirstOrDefault(c => c.Code == code);
        
        //404
        if (coupon == null)         
            return Result.Fail(FailureCode.NotFound).WithError("The coupon doesn't exist or has expired!");

        return MapToDto(coupon);
    }
}

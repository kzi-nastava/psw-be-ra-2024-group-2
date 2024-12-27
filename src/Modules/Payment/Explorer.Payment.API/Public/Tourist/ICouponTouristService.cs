using Explorer.Payment.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Public.Tourist;

public interface ICouponTouristService
{
    public Result<CouponDto> UseCoupon(string code);
    public Result<CouponDto> GetCoupon(string code);
}

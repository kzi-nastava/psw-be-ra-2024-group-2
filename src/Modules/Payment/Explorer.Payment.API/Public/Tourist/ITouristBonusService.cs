using Explorer.Payment.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Public.Tourist
{
    public interface ITouristBonusService
    {
        Result<TouristBonusDto> Create(long touristId, int discountPercetage);
        Result<TouristBonusDto> GetById(long touristId);
        Result<TouristBonusDto> Use(long touristId, string couponCode);
    }
}

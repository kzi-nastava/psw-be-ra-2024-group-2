using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.UseCases.Tourist
{
    public class TouristBonusService : CrudService<TouristBonusDto, TouristBonus>, ITouristBonusService
    {
        private readonly ICrudRepository<TouristBonus> _touristBonusRepository;
        private readonly ICrudRepository<Coupon> _couponRepository;
        private readonly ITransactionRepository _transactionRepository;
        public TouristBonusService(ICrudRepository<TouristBonus> touristBonusRepository,
            ICrudRepository<Coupon> couponRepository,
            ITransactionRepository transactionRepository,
            IMapper mapper) : base(touristBonusRepository, mapper)
        {
            _touristBonusRepository = touristBonusRepository;
            _couponRepository = couponRepository;
            _transactionRepository = transactionRepository;
        }

        public Result<TouristBonusDto> Create(long touristId, int discountPercentage)
        {
            try
            {
                _transactionRepository.BeginTransaction();
                Coupon coupon = new Coupon(-1, -100, discountPercentage, true);
                TouristBonus touristBonus = new TouristBonus(touristId, coupon.Code);

                var existingBonus = _touristBonusRepository.GetPaged(0, 0).Results
                    .FirstOrDefault(tb => tb.TouristId == touristId);

                if (existingBonus != null)
                {
                    throw new ArgumentException($"TouristBonus already exists for TouristId: {touristId}");
                }
                _couponRepository.Create(coupon);
                _touristBonusRepository.Create(touristBonus);
                _transactionRepository.CommitTransaction();
                return Result.Ok(MapToDto(touristBonus));
            }
            catch (KeyNotFoundException e)
            {
                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                _transactionRepository.RollbackTransaction();
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TouristBonusDto> GetById(long touristId)
        {
            try
            {
                var touristBonus = _touristBonusRepository.GetPaged(0, 0).Results
                    .FirstOrDefault(tb => tb.TouristId == touristId);

                if (touristBonus == null)
                {
                    return Result.Fail<TouristBonusDto>("TouristBonus not found for the specified TouristId.");
                }

                return Result.Ok(MapToDto(touristBonus));
            }
            catch (Exception ex)
            {
                return Result.Fail<TouristBonusDto>("An error occurred while retrieving the TouristBonus.")
                             .WithError(ex.Message);
            }
        }

        public Result<TouristBonusDto> Use(long touristId, string couponCode)
        {
            try
            {
                var touristBonus = _touristBonusRepository.GetPaged(0, 0).Results
                    .FirstOrDefault(tb => tb.TouristId == touristId && tb.CouponCode == couponCode);

                if (touristBonus == null)
                    return Result.Fail<TouristBonusDto>($"TouristBonus not found for TouristId: {touristId} and CouponCode: {couponCode}");
                if(touristBonus.CouponCode != couponCode)
                    return Result.Fail<TouristBonusDto>($"TouristBonus code not valid for TouristId: {touristId} and CouponCode: {couponCode}");
                if(touristBonus.IsUsed)
                    return Result.Fail<TouristBonusDto>($"TouristBonus already userd for TouristId: {touristId} and CouponCode: {couponCode}");

                var coupon = _couponRepository.GetPaged(0, 0).Results
                    .FirstOrDefault(c => c.Code == couponCode);
                if (coupon == null)
                    return Result.Fail<TouristBonusDto>($"Coupon not found for CouponCode: {couponCode}");

                touristBonus.IsUsed = true;
                _touristBonusRepository.Update(touristBonus);
                return Result.Ok(MapToDto(touristBonus));
            }
            catch (Exception ex)
            {
                return Result.Fail<TouristBonusDto>("An error occurred while using the TouristBonus.")
                             .WithError(ex.Message);
            }
        }

    }
}

using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class RatingApplicationService : CrudService<RatingApplicationDto,RatingApplication>, IRatingApplicationService
    {
        private readonly ICrudRepository<RatingApplication> _ratingApplicationRepository;

        public RatingApplicationService(ICrudRepository<RatingApplication> repository, IMapper mapper) : base(repository, mapper)
        {
            _ratingApplicationRepository = repository;

        }
        public Result<RatingApplicationDto> Get(long userId)
        {
            var rating = _ratingApplicationRepository.Get(userId);
            if (rating == null)
            {
                return Result.Fail(new Error("Rating not found"));
            }
            return MapToDto(rating);
        }
        


        public Result<RatingApplicationDto> Update(long userId, RatingApplicationDto newRating)
        {
            var rating = _ratingApplicationRepository.Get(userId);
            if (rating == null)
            {
                return Result.Fail(new Error("Rating application not found"));
            }
            try
            {
                var updateRating = new RatingApplication(
                    newRating.Grade,
                    newRating.Comment,
                    newRating.RatingTime,
                    newRating.UserId
                ) ;

                var updatedRating = _ratingApplicationRepository.Update(updateRating);
                return MapToDto(updatedRating);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(new Error(e.Message));
            }
        }

        public Result<RatingApplicationDto> Create(long userId, RatingApplicationDto ratingApplication)
        {
            try
            {
                var pagedResult = base.GetPaged(1, int.MaxValue);
                if (pagedResult.IsFailed)
                {
                    return Result.Fail(pagedResult.Errors);
                }
                var filteredRatings = pagedResult.Value.Results
                    .Any(r => r.UserId == userId);
                if (filteredRatings)
                {
                    return Result.Fail("User already rated applications.");
                }
                var rating = MapToDomain(ratingApplication);
                var results = _ratingApplicationRepository.Create(rating);
                return MapToDto(results);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
    }
}

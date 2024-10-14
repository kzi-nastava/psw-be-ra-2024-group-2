using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
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


        public Result<RatingApplicationDto> Get(long personId)
        {
            var rating = _ratingApplicationRepository.Get(personId);
            if (rating == null)
            {
                return Result.Fail(new Error("Rating not found"));
            }
            return MapToDto(rating);
        }


        public Result<RatingApplicationDto> Update(long personId, RatingApplicationDto newRating)
        {
            var rating = _ratingApplicationRepository.Get(personId);
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
                    newRating.PersonId
                );

                var updatedRating = _ratingApplicationRepository.Update(updateRating);
                return MapToDto(updatedRating);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(new Error(e.Message));
            }
        }
    }
}

using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class TourPreferenceService : CrudService<TourPreferenceDto, TourPreference>, ITourPreferenceService
    {
        public readonly ICrudRepository<TourPreference> _repository;
        public TourPreferenceService(ICrudRepository<TourPreference> repository, IMapper mapper) :
            base(repository, mapper)
        {
            _repository = repository;
        }

        public PagedResult<TourPreferenceDto> GetByTouristId(long userId)
        {
            var allTourPreferences = _repository.GetPaged(1, int.MaxValue);


            var filteredTourPreferences = allTourPreferences.Results
                               .Where(tp => tp.TouristId == userId)
                               .Select(tp => MapToDto(tp))
                               .ToList();

            return new PagedResult<TourPreferenceDto>(filteredTourPreferences, filteredTourPreferences.Count());
        }

        public Result<TourPreferenceDto> Update(TourPreferenceDto tourPreference)
        {
            try
            {
                var existingPreference = _repository.Get(tourPreference.Id);
                if (existingPreference == null) return Result.Fail(FailureCode.NotFound);

                // Provera da li korisnik ažurira svoje preference
                if (existingPreference.TouristId != tourPreference.TouristId)
                    return Result.Fail(FailureCode.InvalidArgument);

                var result = _repository.Update(MapToDomain(tourPreference));
                return Result.Ok(MapToDto(result));
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

    }
}

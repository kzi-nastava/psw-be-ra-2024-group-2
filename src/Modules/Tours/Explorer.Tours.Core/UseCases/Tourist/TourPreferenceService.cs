using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using FluentResults;

public class TourPreferenceService : CrudService<TourPreferenceDto, TourPreference>, ITourPreferenceService
{
    private readonly ICrudRepository<TourPreference> _repository;
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

    public override Result<TourPreferenceDto> Create(TourPreferenceDto tourPreference)
    {
        try
        {
            // Prvo proveravamo da li postoje preference za ovog korisnika
            var existingPreference = _repository.GetPaged(1, int.MaxValue)
                                              .Results
                                              .FirstOrDefault(tp => tp.TouristId == tourPreference.TouristId);

            if (existingPreference != null)
            {
                // Ako postoje, radimo update
                tourPreference.Id = existingPreference.Id; // Postavljamo ID postojećeg
                return Update(tourPreference);
            }

            // Ako ne postoje, kreiramo nove
            var domainModel = MapToDomain(tourPreference);
            var result = _repository.Create(domainModel);
            return Result.Ok(MapToDto(result));
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result<TourPreferenceDto> Update(TourPreferenceDto tourPreference)
    {
        try
        {
            var existingPreference = _repository.Get(tourPreference.Id);
            if (existingPreference == null) return Result.Fail(FailureCode.NotFound);

            if (existingPreference.TouristId != tourPreference.TouristId)
                return Result.Fail(FailureCode.InvalidArgument);

            existingPreference.TouristId = tourPreference.TouristId;
            existingPreference.Difficulty = Enum.Parse<DifficultyLevel>(tourPreference.Difficulty);
            existingPreference.WalkRating = tourPreference.WalkRating;
            existingPreference.CarRating = tourPreference.CarRating;
            existingPreference.BoatRating = tourPreference.BoatRating;
            existingPreference.BicycleRating = tourPreference.BicycleRating;
            existingPreference.Tags = tourPreference.Tags?
            .Select(tag => new TourPreferenceTag(tag))
            .ToList();

            var updatedPreference = _repository.Update(existingPreference);
            return Result.Ok(MapToDto(existingPreference));
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }
}
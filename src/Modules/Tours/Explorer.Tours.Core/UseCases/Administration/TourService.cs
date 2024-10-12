using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration;

public class TourService : CrudService<TourDto,Tour>, ITourService
{

    private readonly ICrudRepository<Tour> _tourRepository;

    public TourService(ICrudRepository<Tour> tourRepository, IMapper mapper) : base(tourRepository, mapper) 
    { 
        _tourRepository = tourRepository;
    }

    public Result<TourDto> AddEquipment(int tourId, EquipmentDto equipment)
    {
        throw new NotImplementedException();
    }

    public Result<TourDto> RemoveEquipment(EquipmentDto equipment)
    {
        throw new NotImplementedException();
    }

    public Result<TourDto> Get(int tourId)
    {
        try
        {
            var tour = _tourRepository.Get(tourId);
            return MapToDto(tour);
        }
        catch
        {
            return Result.Fail(FailureCode.NotFound).WithError("Tour not found");
        }
    }
}

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
    private readonly ICrudRepository<Equipment> _equipmentRepository;

    public TourService(ICrudRepository<Tour> tourRepository, ICrudRepository<Equipment> equipmentRepository, IMapper mapper) : base(tourRepository, mapper) 
    { 
        _tourRepository = tourRepository;
        _equipmentRepository = equipmentRepository;
    }

    public Result<TourDto> UpdateTour(TourDto tourDto, long userId)
    {
        try
        {
            if(tourDto.UserId != userId)
                return Result.Fail(FailureCode.Forbidden).WithError("User is not authorized to add equipment to this tour");

            Tour tour = _tourRepository.Get(tourDto.Id);

            foreach (var elementId in tourDto.Equipment)
            {
                if (tour.Equipment.Any(e => e.Id == elementId))
                {
                    return Result.Fail(FailureCode.InvalidArgument)
                                 .WithError("Equipment is already in the tour!");
                }

            }

            tour.Equipment.Clear();

            foreach (var elementId in tourDto.Equipment)
            {
                var newEquipment = _equipmentRepository.Get(elementId);
                tour.Equipment.Add(newEquipment);
            }

            _tourRepository.Update(tour);
            return MapToDto(tour);
        }
        catch (Exception e)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Tour or equipment doesn't exist !");
        }
    }
}
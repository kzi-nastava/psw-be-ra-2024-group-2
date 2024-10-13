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

    //public Result<TourDto> AddEquipment(long tourId, EquipmentDto equipment, long userId)
    //{
    //    try
    //    { 
    //            var tour = _tourRepository.Get(tourId);
    //            if(tour.UserId == userId)
    //            {
    //                //tour.Equipment.Add(new Equipment(equipment.Name, equipment.Description));
    //                _tourRepository.Update(tour);
    //                return MapToDto(tour);
    //            }
    //            else
    //            {
    //                return Result.Fail(FailureCode.Forbidden).WithError("User is not authorized to add equipment to this tour");
    //            }
    //    }
    //    catch (Exception e)
    //    {
    //        return Result.Fail(FailureCode.NotFound).WithError("Tour doesn't exist!");
    //    }       
    //}

    //public Result<TourDto> RemoveEquipment(long tourId, EquipmentDto equipment, long userId)
    //{
    //    try 
    //    {
    //        var tour = _tourRepository.Get(tourId);
    //        if (tour.UserId == userId)
    //        {
    //            //tour.Equipment.Remove(new Equipment(equipment.Name, equipment.Description));
    //            _tourRepository.Update(tour);
    //            return MapToDto(tour);
    //        }
    //        else
    //        {
    //            return Result.Fail(FailureCode.Forbidden).WithError("User is not authorized to remove equipment from this tour");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        return Result.Fail(FailureCode.NotFound).WithError("Tour doesn't exist!");
    //    }
    //}

    public Result<TourDto> UpdateTour(TourDto tourToUpdate, long userId)
    {
        try
        {
            if (tourToUpdate.UserId == userId)
            {

                Tour tour = _tourRepository.Get(tourToUpdate.Id);

                foreach (int a in tourToUpdate.Equipment)
                {
                    tour.Equipment.Add(a);
                }

                _tourRepository.Update(tour);
                return MapToDto(tour);
            }
            else
            {
                return Result.Fail(FailureCode.Forbidden).WithError("User is not authorized to add equipment to this tour");
            }
        }
        catch (Exception e)
        {
            return Result.Fail(FailureCode.NotFound).WithError("Tour or equipment doesn't exist !");
        }
    }
     
}

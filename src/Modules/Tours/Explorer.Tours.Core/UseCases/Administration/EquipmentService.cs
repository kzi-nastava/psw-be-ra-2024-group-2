using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration;

public class EquipmentService : CrudService<EquipmentDto, Equipment>, IEquipmentService
{
    private readonly ICrudRepository<Equipment> _equipmentRepository;
    private readonly ICrudRepository<TouristEquipment> _touristEquipmentRepository;
    public EquipmentService(ICrudRepository<Equipment> repository,ICrudRepository<TouristEquipment> touristEquipmentRepository, IMapper mapper) : base(repository, mapper) 
    {
        _equipmentRepository = repository;
        _touristEquipmentRepository = touristEquipmentRepository;
    }

    public Result AddEquipmentTourist(long userId, long equipmentId)
    {
        
        var userEquipmentPagedResult = _touristEquipmentRepository.GetPaged(1, int.MaxValue);

        var exists = userEquipmentPagedResult.Results
            .Any(ue => ue.UserId == userId && ue.EquipmentId == equipmentId);

        if (exists)
        {
            return Result.Fail("Equipment already assigned to the user.");
        }

        var touristEquipment = new TouristEquipment(userId, equipmentId);

        _touristEquipmentRepository.Create(touristEquipment);
        return Result.Ok();
    }

    public Result RemoveEquipmentFromTourist(long userId, long equipmentId)
    {
        var userEquipmentPagedResult = _touristEquipmentRepository.GetPaged(1, int.MaxValue);

        var userEquipment = userEquipmentPagedResult.Results
            .FirstOrDefault(ue => ue.UserId == userId && ue.EquipmentId == equipmentId);

        if (userEquipment == null)
        {
            return Result.Fail("Equipment not found for the user.");
        }

        _touristEquipmentRepository.Delete(userEquipment.Id);
        return Result.Ok();
    }


}
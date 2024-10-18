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
    private readonly ICrudRepository<UserEquipment> _userEquipmentRepository;
    public EquipmentService(ICrudRepository<Equipment> repository,ICrudRepository<UserEquipment> userEquipmentRepository, IMapper mapper) : base(repository, mapper) 
    {
        _equipmentRepository = repository;
        _userEquipmentRepository = userEquipmentRepository;
    }

    public Result AddEquipmentToUser(long userId, long equipmentId)
    {
        
        var userEquipmentPagedResult = _userEquipmentRepository.GetPaged(1, int.MaxValue);

        var exists = userEquipmentPagedResult.Results
            .Any(ue => ue.UserId == userId && ue.EquipmentId == equipmentId);

        if (exists)
        {
            return Result.Fail("Equipment already assigned to the user.");
        }

        var userEquipment = new UserEquipment
        {
            UserId = userId,
            EquipmentId = equipmentId
        };

        _userEquipmentRepository.Create(userEquipment);
        return Result.Ok();
    }

    public Result RemoveEquipmentFromUser(long userId, long equipmentId)
    {
        var userEquipmentPagedResult = _userEquipmentRepository.GetPaged(1, int.MaxValue);

        var userEquipment = userEquipmentPagedResult.Results
            .FirstOrDefault(ue => ue.UserId == userId && ue.EquipmentId == equipmentId);

        if (userEquipment == null)
        {
            return Result.Fail("Equipment not found for the user.");
        }

        _userEquipmentRepository.Delete(userEquipment.Id);
        return Result.Ok();
    }


}
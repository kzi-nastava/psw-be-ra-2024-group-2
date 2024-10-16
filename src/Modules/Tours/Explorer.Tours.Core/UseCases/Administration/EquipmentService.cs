using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration;

public class EquipmentService : CrudService<EquipmentDto, Equipment>, IEquipmentService
{
    private readonly ICrudRepository<Equipment> _equipmentRepository;
    private readonly IUserRepository _userRepository;

    public EquipmentService(ICrudRepository<Equipment> equipmentRepository, IUserRepository userRepository, IMapper mapper)
        : base(equipmentRepository, mapper)
    {
        _equipmentRepository = equipmentRepository;
        _userRepository = userRepository;
    }

    public Result AddEquipmentToTourist(int touristId, int equipmentId)
    {
        var equipment = _equipmentRepository.Get(equipmentId);
        if (equipment == null)
        {
            return Result.Fail("Equipment not found.");
        }

        var user = _userRepository.GetById(touristId);
        if (user == null || user.Role != UserRole.Tourist)
        {
            return Result.Fail("Tourist not found.");
        }

        if (user is Tourist tourist)
        {
            tourist.AddEquipment(equipmentId);
            _userRepository.Update(user);
            return Result.Ok();
        }

        return Result.Fail("User is not a tourist.");
    }


    public Result RemoveEquipmentFromTourist(int touristId, int equipmentId)
    {
        var user = _userRepository.GetById(touristId);
        if (user == null || user.Role != UserRole.Tourist)
        {
            return Result.Fail("Tourist not found.");
        }

        if (user is Tourist tourist)
        {
            tourist.RemoveEquipment(equipmentId);
            _userRepository.Update(user);
            return Result.Ok();
        }

        return Result.Fail("User is not a tourist.");
    }


}
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Administration;

public class EquipmentService : CrudService<EquipmentDto, Equipment>, IEquipmentService
{
    private readonly ICrudRepository<Equipment> _equipmentRepository;
    private readonly IMapper _mapper;

    public EquipmentService(ICrudRepository<Equipment> repository, IMapper mapper)
        : base(repository, mapper)
    {
        _equipmentRepository = repository;
        _mapper = mapper;
    }

    public Result AddEquipmentTourist(long userId, long equipmentId)
    {
        // TODO: Popraviti ovo
        throw new NotImplementedException();
    }

    public IList<EquipmentDto> GetEquipmentForTourist(long touristId)
    {
        // TODO: Popraviti ovo
        throw new NotImplementedException();
    }

    public Result RemoveEquipmentFromTourist(long userId, long equipmentId)
    {
        // TODO: Popraviti ovo
        throw new NotImplementedException();
    }


    //public Result AddEquipmentTourist(long userId, long equipmentId)
    //{
    //    var userEquipmentPagedResult = _equipmentService.GetFullPaged(1, int.MaxValue);

    //    var exists = userEquipmentPagedResult.Value.Results
    //        .Any(ue => ue.UserId == userId && ue.EquipmentId == equipmentId);

    //    if (exists)
    //    {
    //        return Result.Fail("Equipment already assigned to the user.");
    //    }

    //    var touristEquipment = new Equipment(userId, equipmentId);

    //    _touristEquipmentRepository.Create(touristEquipment);
    //    return Result.Ok();
    //}

    //public Result RemoveEquipmentFromTourist(long userId, long equipmentId)
    //{
    //    var userEquipmentPagedResult = _equipmentService.GetFullPaged(1, int.MaxValue);

    //    var userEquipment = userEquipmentPagedResult.Value.Results
    //        .FirstOrDefault(ue => ue.UserId == userId && ue.EquipmentId == equipmentId);

    //    if (userEquipment == null)
    //    {
    //        return Result.Fail("Equipment not found for the user.");
    //    }

    //    _touristEquipmentRepository.Delete(userEquipment.Id);
    //    return Result.Ok();
    //}
    //public IList<EquipmentDto> GetEquipmentForTourist(long touristId)
    //{

    //    var equipmentIds = _equipmentService.GetFullPaged(1, int.MaxValue)
    //        .Value.Results
    //        .Where(te => te.UserId == touristId)
    //        .Select(te => te.EquipmentId)
    //        .ToList();


    //    var equipmentList = _equipmentRepository.GetPaged(1, int.MaxValue)
    //        .Results
    //        .Where(e => equipmentIds.Contains((int)e.Id))
    //        .ToList();

    //    var equipmentDtoList = _mapper.Map<IList<EquipmentDto>>(equipmentList);
    //    return equipmentDtoList;
    //}
}
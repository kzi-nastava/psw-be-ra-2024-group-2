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
    private readonly IMapper _mapper;

    public EquipmentService(ICrudRepository<Equipment> repository, ICrudRepository<TouristEquipment> touristEquipmentRepository, IMapper mapper)
        : base(repository, mapper)
    {
        _equipmentRepository = repository;
        _touristEquipmentRepository = touristEquipmentRepository;
        _mapper = mapper;
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
    public IList<EquipmentDto> GetEquipmentForTourist(long touristId)
    {

        var equipmentIds = _touristEquipmentRepository.GetPaged(1, int.MaxValue)
            .Results
            .Where(te => te.UserId == touristId)
            .Select(te => te.EquipmentId)
            .ToList();


        var equipmentList = _equipmentRepository.GetPaged(1, int.MaxValue)
            .Results
            .Where(e => equipmentIds.Contains(e.Id))
            .ToList();

        var equipmentDtoList = _mapper.Map<IList<EquipmentDto>>(equipmentList);
        return equipmentDtoList;
    }




}
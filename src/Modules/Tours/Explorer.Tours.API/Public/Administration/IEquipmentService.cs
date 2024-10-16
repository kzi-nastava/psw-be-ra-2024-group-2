﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Administration;

public interface IEquipmentService
{
    Result<PagedResult<EquipmentDto>> GetPaged(int page, int pageSize);
    Result<EquipmentDto> Create(EquipmentDto equipment);
    Result<EquipmentDto> Update(EquipmentDto equipment);
    Result Delete(int id);
    Result AddEquipmentToTourist(int touristId, int equipmentId);
    Result RemoveEquipmentFromTourist(int touristId, int equipmentId);

}
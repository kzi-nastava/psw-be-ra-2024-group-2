using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration;

public interface ITourService
{
    Result<TourDto> AddEquipment(int tourId, EquipmentDto equipment);
    Result<TourDto> RemoveEquipment(EquipmentDto equipment);
    Result<TourDto> Get(int tourId);
}

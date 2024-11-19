using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface IPersonalDairyService
    {
        Result<PersonalDairyDto> Create(PersonalDairyDto dairy);
        Result<PagedResult<PersonalDairyDto>> GetPaged(int page, int pageSize);
        Result<PersonalDairyDto> Get(int id);
        Result<PersonalDairyDto> Update(long id, PersonalDairyDto dairy);
        Result Delete(long id);
        Result<IEnumerable<PersonalDairyDto>> GetAllForUser(long userId);
    }
}

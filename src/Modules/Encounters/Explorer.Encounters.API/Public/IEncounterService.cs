using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public
{
    public interface IEncounterService
    {
        Result<EncounterDTO> Get(long id);
        Result<PagedResult<EncounterDTO>> GetPaged(int page,int size);
        Result<EncounterDTO> Create(EncounterDTO encounter);
        Result<EncounterDTO> Update(EncounterDTO encounter);
        Result Delete(long id);
    }
}
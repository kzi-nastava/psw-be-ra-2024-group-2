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
        //Add encounter methods
        Result<SocialEncounterDto> CreateSocialEncounter(SocialEncounterDto encounter);
        Result<HiddenLocationEncounterDto> CreateHiddenLocationEncounter(HiddenLocationEncounterDto encounter);
        Result<MiscEncounterDto> CreateMiscEncounter(MiscEncounterDto encounter);

        // Update Encounter methods
        Result<SocialEncounterDto> UpdateSocialEncounter(UnifiedEncounterDto encounter);
        Result<HiddenLocationEncounterDto> UpdateHiddenLocationEncounter(UnifiedEncounterDto encounter);
        Result<MiscEncounterDto> UpdateMiscEncounter(UnifiedEncounterDto encounter);

        // Retrieve Encounters
        Result<EncounterDto> GetByName(string name);

        Result<EncounterDto> GetById(long id);

        Result<List<UnifiedEncounterDto>> GetAll();
        Result Delete(long id);
        Result<string> RemoveFromSocialEncounters(int id);
    }
}

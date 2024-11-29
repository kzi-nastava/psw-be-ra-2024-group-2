using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces
{
    public interface IEncounterRepository
    {
        //adding encounters of different types
        void AddEncounter(Encounter encouter);

        //update for different encounter types
        void UpdateEncounter(Encounter encouter);

        Encounter? GetByName(string name);
        List<Encounter> GetAll();
        void Delete(long id);
        Encounter? GetById(long id);
    }
}


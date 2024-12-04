using System;
using System.Collections.Generic;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class EncounterRepository : IEncounterRepository
    {
        private readonly EncountersContext _context;

        public EncounterRepository(EncountersContext context)
        {
            _context = context;
        }

        // Retrieve all Encounters
        public List<Encounter> GetAll()
        {
           return _context.Encounters.ToList();    
        }


        // Add a new Encounter (for each type, use specific methods)
        public void AddEncounter(Encounter encounter)
        {
            _context.Encounters.Add(encounter);
            _context.SaveChanges();
        }


        public void UpdateEncounter(Encounter encounter)
        {
            var existingItem = _context.Encounters.Find(encounter.Id);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(encounter);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Encounter not found.");
            }
        }

        // Delete an Encounter by Id (generic method)
        public void Delete(long id)
        {
            var encounter = _context.Encounters.Find(id);
            if (encounter != null)
            {
                _context.Set<Encounter>().Remove(encounter);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Encounter not found.");
            }
        }

        // Retrieve an Encounter by its Name
        public Encounter? GetByName(string name)
        {
            return _context.Encounters.FirstOrDefault(e => e.Name == name);
        }

        // Retrieve specific Encounter by Id
        public Encounter? GetById(long id)
        {
            return _context.Set<Encounter>().Find(id);
        }
    }
}


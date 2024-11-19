using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain
{
    public class Encounter : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Encounter(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public Encounter()
        {

        }
    }
}

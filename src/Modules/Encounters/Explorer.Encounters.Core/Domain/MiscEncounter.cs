using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain
{
    public class MiscEncounter : Encounter
    {
        public string ActionDescription { get; set; } // Description of the action the tourist needs to perform (e.g., "Do 20 push-ups")

        public MiscEncounter(string name, string description, string actionDescription)
            : base(name, description)
        {
            ActionDescription = actionDescription;
        }

        public MiscEncounter() { }
    }
}
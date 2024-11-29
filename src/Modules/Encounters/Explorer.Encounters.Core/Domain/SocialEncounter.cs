using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Explorer.Encounters.Core.Domain
{
    public class SocialEncounter : Encounter
    {
        public int RequiredPeople { get; set; } // Number of people required to solve the challenge
        public double RangeInMeters { get; set; } // The radius (in meters) within which the tourists need to be

        public SocialEncounter(string name, string description, int requiredPeople, double rangeInMeters, double lattitude, double longitude)
            : base(name, description, lattitude, longitude)
        {
            RequiredPeople = requiredPeople;
            RangeInMeters = rangeInMeters;
        }

        public SocialEncounter() { }
    }
}


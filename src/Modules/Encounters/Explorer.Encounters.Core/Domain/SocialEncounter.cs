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
        public List<int> TouristsInRange { get; set; } = new List<int>();

        public SocialEncounter(string name, string description, int requiredPeople, double rangeInMeters, double lattitude, double longitude, bool isForTour, long? tourId)
            :   base(name, description, lattitude, longitude, isForTour, tourId)
        {
            RequiredPeople = requiredPeople;
            RangeInMeters = rangeInMeters;
        }

        public SocialEncounter() { }
        public List<int> AddTouristInRange(int touristId)
        {
            if (TouristIds.Contains(touristId))
            {
                return null;
            }

            if (!TouristsInRange.Contains(touristId))
            {
                TouristsInRange.Add(touristId);
            }

            if (TouristsInRange.Count == RequiredPeople)
            {
                CompleteEncounter();
                return TouristIds; 
            }

            return null;
        }
        private void CompleteEncounter()
        {
            TouristIds.AddRange(TouristsInRange);
            TouristsInRange.Clear();
        }
    }
}


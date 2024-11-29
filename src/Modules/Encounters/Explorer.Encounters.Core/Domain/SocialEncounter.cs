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
        public double Longitude {  get; set; }
        public double Latitude { get; set; }
        public List<int> TouristIds { get; set; } = new List<int>();
        public SocialEncounter(string name, string description, int requiredPeople, double rangeInMeters, double latitude, double longitude)
            : base(name, description)
        {
            RequiredPeople = requiredPeople;
            RangeInMeters = rangeInMeters;
            Longitude = longitude;
            Latitude = latitude;
        }

        public SocialEncounter() { }

        public void UpdateTouristIds(List<int> touristIds)
        {
            TouristIds = touristIds;
        }

        public bool CheckPoisition(double touristLongitude, double touristLatitude)
        {
            const double earthRadius = 6371000; // Earth's radius in meters
            double dLat = DegreesToRadians(touristLatitude - Latitude);
            double dLon = DegreesToRadians(touristLongitude - Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(Latitude)) * Math.Cos(DegreesToRadians(touristLatitude)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadius * c;

            Console.WriteLine("Calculated Distance: " + distance); // Debug line

            return distance <= 15; // Check if distance is within 15 meters
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}


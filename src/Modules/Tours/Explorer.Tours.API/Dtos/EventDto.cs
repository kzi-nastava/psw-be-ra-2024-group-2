using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public EventImageDto? Image { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List <EventAcceptionDto> EventAcceptions { get; set; } = new List<EventAcceptionDto> { };

        public bool CheckRadius(double otherLongitude, double otherLatitude)
        {
            const double earthRadius = 6371000; // Earth's radius in meters
            double dLat = DegreesToRadians(otherLatitude - Latitude);
            double dLon = DegreesToRadians(otherLongitude - Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(Latitude)) * Math.Cos(DegreesToRadians(otherLatitude)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadius * c;

            Console.WriteLine("Calculated Distance: " + distance); // Debug line

            return distance <= 10000; // Check if distance is within 50 meters
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}

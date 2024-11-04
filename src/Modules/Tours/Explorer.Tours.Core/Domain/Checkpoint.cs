using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Explorer.BuildingBlocks.Core.Domain;


namespace Explorer.Tours.Core.Domain
{
    public class Checkpoint : Entity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; } 
        public string Name { get; set; }      
        public string Description { get; set; }
        public long? ImageId {  get; set; }
        public Image? Image { get; set; }
        public List<Tour> Tours { get; set; } = new List<Tour>();
        public string Secret {  get; set; }
        public Checkpoint(double latitude, double longitude, string name, string description, long? imageId, string secret)
        {
            Validate(latitude, longitude, name, description);

            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            ImageId = imageId;
            Secret = secret;
        }
        public Checkpoint(double latitude, double longitude, string name, string description, string secret)
        {
            Validate(latitude, longitude, name, description);

            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            Secret = secret;
        }

        public Checkpoint(double latitude, double longitude, string name, string description, Image ?image, string secret)
        {
            Validate(latitude, longitude, name, description);

            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            Image = image;
            Secret = secret;
        }


        public Checkpoint() { }
        private void Validate(double latitude, double longitude, string name, string description)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException("Latitude must be between -90 and 90 degrees.");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentException("Longitude must be between -180 and 180 degrees.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }

            if (description == null)
            {
                throw new ArgumentException("Description cannot be null.");
            }
        }

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

            return distance <= 150; // Check if distance is within 50 meters
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

    }
}

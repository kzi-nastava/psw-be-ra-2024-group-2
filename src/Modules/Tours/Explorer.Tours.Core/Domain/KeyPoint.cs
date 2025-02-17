using System.Xml.Linq;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class KeyPoint : Entity
    {
        public decimal Latitude { get; private set; }  // 'lat' (latitude of the location)
        public decimal Longitude { get; private set; } // 'lng' (longitude of the location)
        public string Name { get; private set; }       // 'name' (name of the key point)
        public string Description { get; private set; } // 'description' (description of the key point)
        public long TourId { get; private set; }

        // Constructor to initialize a KeyPoint
        public KeyPoint(decimal latitude, decimal longitude, string name, string description, long tourId)
        {
            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            TourId = tourId;
            Validate();
        }

        // Validation method to ensure all required fields are valid
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid name for the key point");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid description for the key point");
            if (Latitude < -90 || Latitude > 90) throw new ArgumentException("Latitude must be between -90 and 90");
            if (Longitude < -180 || Longitude > 180) throw new ArgumentException("Longitude must be between -180 and 180");           
        }

        public void UpdateKeyPoint(decimal latitude, decimal longitude, string name, string description)
        {
            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            Validate();
        }
    }
}

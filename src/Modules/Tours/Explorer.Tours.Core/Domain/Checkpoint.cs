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
        public Checkpoint(double latitude, double longitude, string name, string description, long? imageId)
        {
            Validate(latitude, longitude, name, description);

            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            ImageId = imageId;
        }
        public Checkpoint(double latitude, double longitude, string name, string description)
        {
            Validate(latitude, longitude, name, description);

            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
        }

        public Checkpoint(double latitude, double longitude, string name, string description, Image ?image)
        {
            Validate(latitude, longitude, name, description);

            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            Image = image;
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
    }
}

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
            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            ImageId = imageId;
            Validate();
        }
        public Checkpoint(double latitude, double longitude, string name, string description)
        {
            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            Validate();
        }

        public Checkpoint() { }
        private void Validate()
        {
            if (Latitude < -90 || Latitude > 90)
            {
                throw new ArgumentException("Latitude must be between -90 and 90 degrees.");
            }

            if (Longitude < -180 || Longitude > 180)
            {
                throw new ArgumentException("Longitude must be between -180 and 180 degrees.");
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }

            if (Description == null)
            {
                throw new ArgumentException("Description cannot be null.");
            }
        }
    }
}

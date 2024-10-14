using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;


namespace Explorer.Tours.Core.Domain
{
    public class Checkpoint : Entity
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; } 
        public string Name { get; private set; }      
        public string Description { get; private set; }    
        public int? PhotoId { get; private set; }

        public Checkpoint(double latitude, double longitude, string name, string description, int? photoId)
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

            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            Description = description;
            PhotoId = photoId;
        }
    }
}

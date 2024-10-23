using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Tours.Core.Domain
{
    public class TourObject : Entity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public ObjectCategory Category { get; private set; }
        public long? ImageId { get;  set; }
        public Image? Image { get;  set; }

        public double Longitude { get;  set; } 
        public double Latitude { get;  set; }

    public TourObject(string name, string description, ObjectCategory category, long imageId, double lng, double lat)
        {   
            Name = name;
            Description = description;
            Category = category;
            ImageId = imageId;
            Longitude = lng;
            Latitude = lat;
            Validate();
        }

    public TourObject(string name, string description, ObjectCategory category, double longitude, double latitude)
        {
            Name = name;
            Description = description;
            Category = category;
            Longitude = longitude;
            Latitude = latitude;
            Validate();
        }

        public string GetObjectCategoryNormalized => Category switch
        {
            ObjectCategory.WC => "WC",
            ObjectCategory.Restaurant => "Restaurant",
            ObjectCategory.Parking => "Parking",
            _ => throw new ArgumentException("Category is not valid")
        };

        public ObjectCategory GetObjectCategoryDenormalized(string category) => category switch
        {
            "WC" => ObjectCategory.WC,
            "Restaurant" => ObjectCategory.Restaurant,
            "Parking" => ObjectCategory.Parking,
            _ => throw new ArgumentException("Category is not valid")
        };

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name.");
            if (!Enum.IsDefined(typeof(ObjectCategory), Category)) throw new ArgumentException("Invalid object category.");
            if (double.IsNaN(Longitude)) throw new ArgumentException("Invalid location.");
            if (double.IsNaN(Latitude)) throw new ArgumentException("Invalid location");
        }

        
    }



    public enum ObjectCategory
    {
        WC, 
        Restaurant,
        Parking
    }
}

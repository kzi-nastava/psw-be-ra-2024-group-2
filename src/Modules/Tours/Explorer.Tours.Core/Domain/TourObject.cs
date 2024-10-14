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
        public long? ImageId { get; private set; }
        public Image? Image { get; private set; }

    public TourObject(string name, string description, ObjectCategory category, long imageId)
        {   
            Name = name;
            Description = description;
            Category = category;
            ImageId = imageId;
        }

    public TourObject(string name, string description, ObjectCategory category)
        {
            Name = name;
            Description = description;
            Category = category;
            Validate();
        }
       /* public string GetObjectCategorzNormalized => ObjectCategory switch
        {
            ObjectCategory.WC => "WC",
            ObjectCategory.Restaurant => "Restaurant",
            ObjectCategory.Parking => "Parking",
            _ => throw new ArgumentException("MimeType is not valid")
        };


        private ObjectCategory GetObjectCategoryDenormalized(string category) => ObjectCategory switch
        {
            "WC" => MimeType.Jpeg,
            "Resta" => MimeType.Png,
            "image/gif" => MimeType.Gif,
            _ => throw new ArgumentException("MimeType is not valid")
        };*/
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name.");
            if (!Enum.IsDefined(typeof(ObjectCategory), Category)) throw new ArgumentException("Invalid object category.");
        }

        
    }



    public enum ObjectCategory
    {
        WC, 
        Restaurant,
        Parking
    }
}

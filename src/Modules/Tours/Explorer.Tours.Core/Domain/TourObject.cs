using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourObject : Entity
    {
        // Add images when finished generic image class

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public ObjectCategory Category { get; private set; }

        public TourObject(string name, string description, ObjectCategory category)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            
            Name = name;
            Description = description;
            Category = category;
        }
    }

    public enum ObjectCategory
    {
        WC, 
        Restaurant,
        Parking
    }
}

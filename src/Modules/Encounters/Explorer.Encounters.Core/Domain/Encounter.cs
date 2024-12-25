using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain
{
    public class Encounter : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public bool IsForTour { get; set; }
        public long? TourId { get; set; }
        public List<int> TouristIds { get; set; } = new List<int>();
        public Encounter(string name, string description, double lattitude, double longitude, bool isForTour, long? tourId)
        {
            Name = name;
            Description = description;
            Lattitude = lattitude;
            Longitude = longitude;
            IsForTour = isForTour;
            TourId = tourId;
        }
        public Encounter()
        {
        }
    }
}

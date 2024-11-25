using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain
{
    public class HiddenLocationEncounter : Encounter
    {
        public Image Image { get; set; } 
        public double TargetLatitude { get; set; } // Latitude of the location from which the image was taken
        public double TargetLongitude { get; set; } // Longitude of the location
        public double RangeInMeters { get; set; } // Radius (in meters) within which the challenge can be activated

        public HiddenLocationEncounter(string name, string description, Image image, double targetLatitude, double targetLongitude, double rangeInMeters)
            : base(name, description)
        {
            Image = image;
            TargetLatitude = targetLatitude;
            TargetLongitude = targetLongitude;
            RangeInMeters = rangeInMeters;
        }

        public HiddenLocationEncounter() { }
    }
}

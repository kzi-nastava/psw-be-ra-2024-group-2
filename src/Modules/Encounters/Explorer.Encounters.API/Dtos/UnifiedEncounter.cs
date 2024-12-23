using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class UnifiedEncounterDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<int> TouristIds { get; set; } = new List<int>();

        // Common to all encounters
        public string EncounterType { get; set; } = string.Empty; // "Social", "HiddenLocation", "Misc"

        // Properties for Social Encounter
        public int? RequiredPeople { get; set; } // Nullable because it's specific to Social encounters
        public double? SocialRangeInMeters { get; set; } // Nullable because it's specific to Social encounters
        public List<int> TouristsInRange { get; set; } = new List<int>(); // Nullable because it's specific to Social encounters

        // Properties for Hidden Location Encounter
        public EncounterImageDto? Image { get; set; } // Nullable because it's specific to Hidden Location encounters
        public double? TargetLatitude { get; set; } // Nullable because it's specific to Hidden Location encounters
        public double? TargetLongitude { get; set; } // Nullable because it's specific to Hidden Location encounters
        public double? HiddenLocationRangeInMeters { get; set; } // Nullable because it's specific to Hidden Location encounters

        // Properties for Misc Encounter
        public string? ActionDescription { get; set; } // Nullable because it's specific to Misc encounters
    }

}

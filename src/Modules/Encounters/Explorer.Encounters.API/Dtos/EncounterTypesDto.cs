using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
  
    // Social Encounter DTO
    public class SocialEncounterDto : EncounterDto
    {
        public int RequiredPeople { get; set; }
        public double RangeInMeters { get; set; } // Radius in meters
        public List<int> TouristsInRange { get; set; } = new List<int>();
    }

    // Hidden Location Encounter DTO
    public class HiddenLocationEncounterDto : EncounterDto
    {
        public EncounterImageDto Image { get; set; } = default!;
        public double TargetLatitude { get; set; }
        public double TargetLongitude { get; set; }
        public double RangeInMeters { get; set; }
    }

    // Misc Encounter DTO
    public class MiscEncounterDto : EncounterDto
    {
        public string ActionDescription { get; set; } = string.Empty;
    }
}

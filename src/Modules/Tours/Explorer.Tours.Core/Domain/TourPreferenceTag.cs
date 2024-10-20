using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;

public class TourPreferenceTag : Entity
{
    public string Name { get; private set; }

    public TourPreferenceTag(string name)
    {
        Name = name;
        Validate();
    }

    private void Validate()
    {
        if (Name != "Adventure" && Name != "Relaxation" && Name != "Historical" &&
            Name != "Cultural" && Name != "Nature")
        {
            throw new ArgumentException($"Invalid name: {Name}. Allowed values are Adventure, Relaxation, Historical, Cultural, and Nature.");
        }
    }
}

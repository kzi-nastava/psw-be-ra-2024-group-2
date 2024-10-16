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
        // TODO: Implement validation
        throw new NotImplementedException();
    }
}

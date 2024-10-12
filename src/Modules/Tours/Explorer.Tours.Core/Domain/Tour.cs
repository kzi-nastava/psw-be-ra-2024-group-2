using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class Tour : Entity
{
    public List<Equipment> Equipment { get; private set; }

    public Tour(List<Equipment> equipment)
    {
        Equipment = equipment;
    }
}

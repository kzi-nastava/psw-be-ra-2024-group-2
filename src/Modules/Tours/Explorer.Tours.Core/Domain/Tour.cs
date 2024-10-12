using Explorer.BuildingBlocks.Core.Domain;


namespace Explorer.Tours.Core.Domain;

public class Tour : Entity
{
    public long UserId { get; private set; }
    public List<Equipment> Equipment { get; private set; }

    public Tour(long userid ,List<Equipment> equipment)
    {
        UserId = userid;
        Equipment = equipment;
    }
}

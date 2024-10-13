using Explorer.BuildingBlocks.Core.Domain;


namespace Explorer.Tours.Core.Domain;

public class Tour : Entity
{
    public long UserId { get; private set; }
    public List<int> Equipment { get; private set; } = new List<int>();

    public Tour(long userId, List<int> equipment)
    {
        UserId = userId;
        Equipment = equipment;
    }
}

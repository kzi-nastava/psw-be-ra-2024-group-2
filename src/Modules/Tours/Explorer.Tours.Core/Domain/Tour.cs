using Explorer.BuildingBlocks.Core.Domain;


namespace Explorer.Tours.Core.Domain;

public class Tour : Entity
{
    public long UserId { get; private set; }
    public List<Equipment> Equipment { get; private set; } = new List<Equipment>();

    public Tour() { }

    public Tour(long userId)
    {
        UserId = userId;
    }
}

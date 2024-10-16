using Explorer.Stakeholders.Core.Domain;

public class Tourist : User
{
    public List<int> EquipmentIds { get; private set; } = new List<int>();

    public Tourist(string username, string password, bool isActive)
        : base(username, password, UserRole.Tourist, isActive)
    {
    }

    public void AddEquipment(int equipmentId)
    {
        if (EquipmentIds.Contains(equipmentId))
            throw new InvalidOperationException("Equipment already added.");
        EquipmentIds.Add(equipmentId);
    }

    public void RemoveEquipment(int equipmentId)
    {
        if (!EquipmentIds.Contains(equipmentId))
            throw new InvalidOperationException("Equipment not found.");
        EquipmentIds.Remove(equipmentId);
    }
}

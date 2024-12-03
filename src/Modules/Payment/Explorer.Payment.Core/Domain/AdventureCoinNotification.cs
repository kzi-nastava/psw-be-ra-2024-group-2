using Explorer.BuildingBlocks.Core.Domain;

public class AdventureCoinNotification : Entity
{
    public long TouristId { get; set; }
    public bool IsRead { get; set; } 
    public DateTime SentAt { get; set; } 

    public AdventureCoinNotification(long touristId)
    {
        TouristId = touristId;
        IsRead = false; 
        SentAt = DateTime.UtcNow;
    }
    public AdventureCoinNotification() { }
    public void MarkAsRead()
    {
        IsRead = true;
    }
}

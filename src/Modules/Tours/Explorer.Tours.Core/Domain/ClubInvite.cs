using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.Core.Domain;

public class ClubInvite : Entity
{
    public long OwnerId { get; set; }
    public long TouristId { get; set; }
    public long ClubId { get; set; }
    public DateTime Date { get; set; }
    public TourInviteStatus Status { get; set; }
    public ClubInvite(long ownerId, long touristId, DateTime date, TourInviteStatus status)
    {
        OwnerId = ownerId;
        TouristId = touristId;
        Date = date;
        Status = status;
        Validate();
    }

    private void Validate()
    {
        if (!Enum.IsDefined(typeof(TourInviteStatus), Status)) throw new ArgumentException("Status is not valid");

    }
}
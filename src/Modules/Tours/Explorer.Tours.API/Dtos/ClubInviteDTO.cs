using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.API.Dtos;

public class ClubInviteDTO
{
    public long OwnerId { get; set; }
    public long TouristId { get; set; }
    public long ClubId { get; set; }
    public long UserId { get; set; }
    public DateTime Date { get; set; }
    public TourInviteStatus Status { get; set; }
}
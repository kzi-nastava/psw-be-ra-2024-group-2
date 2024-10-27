using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.API.Dtos;

public class TourDto
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public List<int> Equipment { get; set; } = new List<int>();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TourDifficulty Difficulty { get; set; }
    public TourTag Tag { get; set; }
    public TourStatus Status { get; set; }
    public double Price { get; set; }
    public List<int> Checkpoints { get; set; } = new List<int>();
    public List<TourDurationByTransportDto>? TourDurationByTransportDtos { get; set; } = new List<TourDurationByTransportDto>();
    public TouristPositionDto? TouristPosition { get; set; }
}


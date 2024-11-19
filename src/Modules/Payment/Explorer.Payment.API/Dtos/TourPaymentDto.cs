using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos;

public class TourPaymentDto
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
    public List<TourDurationByTransportPaymentDto>? TourDurationByTransportDtos { get; set; } = new List<TourDurationByTransportPaymentDto>();
}

using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos;

public sealed class TourSouvenirDto
{
    public long? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Count { get; set; }
    public SouvenirStatus? SouvenirStatus { get; set; }
    public PaymentImageDto ImageDto { get; set; }
    public long TourId { get; set; }
}

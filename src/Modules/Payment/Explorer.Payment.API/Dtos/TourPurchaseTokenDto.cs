using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos;

public class TourPurchaseTokenDto : Entity
{
    public long UserId { get; set; }
    public long TourId { get; set; }
    public double Price { get; set; }
    public DateTime PurchaseTime { get; set; }

}

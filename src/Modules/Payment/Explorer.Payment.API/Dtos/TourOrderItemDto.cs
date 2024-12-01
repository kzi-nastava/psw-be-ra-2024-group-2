using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos;

public class TourOrderItemDto : TourOrderItemBasicDto
{
    public long TourId { get; set; }
}

public class BundleOrderItemDto1 : TourOrderItemBasicDto
{
    public long BundleId { get; set; }
}
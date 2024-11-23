using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos;

public class CouponDto
{
    public string Code { get; set; }
    public int DiscountPercentage { get; set; }
    public long TourId { get; set; }
    public long AuthorId { get; set; }
    public Boolean AllToursDiscount { get; set; }
}

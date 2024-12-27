using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos
{
    public class TouristBonusDto
    {
        public long TouristId { get; set; }
        public string? CouponCode { get; set; }
        public bool IsUsed { get; set; }
    }
}

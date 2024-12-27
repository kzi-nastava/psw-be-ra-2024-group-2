using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain
{
    public class TouristBonus : Entity
    {
        public long TouristId { get; set; }
        public string CouponCode { get; set; }
        public bool IsUsed { get; set; }

        public TouristBonus(long touristId, string couponCode)
        {
            TouristId = touristId;
            CouponCode = couponCode;
            IsUsed = false;
        }
    }
}
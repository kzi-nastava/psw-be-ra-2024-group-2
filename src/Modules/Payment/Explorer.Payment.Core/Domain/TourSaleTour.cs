using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain
{
    public class TourSaleTour : Entity
    {
        public long TourSaleId { get; set; }
        public TourSale TourSale { get; set; }
        public long TourId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos
{
    public class TourPricesDto
    {
        public int TourId { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos
{
    public class TourSaleDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public double DiscountPercentage { get; set; }
        public List<TourDtoPayment> Tours { get; set; } = new List<TourDtoPayment>();
        public List<TourPricesDto> Prices { get; set; } = new List<TourPricesDto>();
    }
}

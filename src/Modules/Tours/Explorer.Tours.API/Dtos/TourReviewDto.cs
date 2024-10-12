using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourReviewDto
    {
        public int Grade {  get; set; }

        public string? Comment { get; set; }

        public int UserId { get; set; }

        public DateOnly ReviewDate { get; set; }

        public DateOnly VisitDate { get; set; }

        public int TourId { get; set; } 

    }
}

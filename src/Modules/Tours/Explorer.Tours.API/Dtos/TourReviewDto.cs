﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourReviewDto
    {
        public long Id { get; set; }
        public int Grade {  get; set; }

        public string? Comment { get; set; }

        public long UserId { get; set; }

        public long TourId { get; set; }

        public TourImageDto? Image { get; set; }

        public DateTime ReviewDate { get; set; }

        public DateTime VisitDate { get; set; }

        public double Progress { get; set; }

        //public int TourId { get; set; } 

    }
}

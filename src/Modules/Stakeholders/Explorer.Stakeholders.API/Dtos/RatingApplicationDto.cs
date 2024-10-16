﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class RatingApplicationDto
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public string? Comment { get; set; }
        public DateTime RatingTime { get; set; }
        public long UserId { get; set; }


    }
}

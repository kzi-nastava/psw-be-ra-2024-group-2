﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class ClubDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ImageId { get; set; }
        public int? OwnerId { get; set; }
    }
}

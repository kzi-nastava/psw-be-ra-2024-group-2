﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TourDifficulty Difficulty { get; set; }
        public TourTag Tag { get; set; }
        public TourStatus Status { get; set; }
        public double Price { get; set; }

        public enum TourStatus
        {
            Draft,
            Published,
            Archived
        }

        public enum TourTag
        {
            Adventure,
            Relaxation,
            Historical,
            Cultural,
            Nature
        }

        public enum TourDifficulty
        {
            Easy,
            Moderate,
            Hard
        }
    }
}

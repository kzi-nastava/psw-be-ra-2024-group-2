using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourPreferenceDto
    {
        public long Id { get; set; }
        public long TouristId { get; set; }
        public string Difficulty { get; set; }
        public int WalkRating { get; set; }
        public int BicycleRating { get; set; }
        public int CarRating { get; set; }
        public int BoatRating { get; set; }
        public List<string>? Tags { get; set; }
    }
}

using System;


namespace Explorer.Tours.API.Dtos
{
    public class TourRateDto
    {
        public long? Id { get; set; }
        public long TourId { get; set; }
        public long TouristId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string? TouristUsername { get; set; }

        /*
        public TourRateDto MapToDto(Tour tourRate)
        {
            return new TourRateDto
            {
                Id = tourRate.Id,
                TourId = tourRate.TourId,
                TouristId = tourRate.TouristId,
                Rating = tourRate.Rating,
                Comment = tourRate.Comment,
            };
        }
        */

    }
}

using System;

using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.Core.Domain
{
    public class TourRate : Entity
    {
        public long TourId { get; set; }

        public long TouristId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public string TouristUsername { get; set; }


        public TourRateDto MapToDto()
        {
            return new TourRateDto
            {
                Id = this.Id,
                TourId = this.TourId,
                TouristId = this.TouristId,
                Rating = this.Rating,
                Comment = this.Comment,
                TouristUsername = this.TouristUsername
            };
        }

        public TourRate(TourRateDto dto)
        {
            TourId = dto.TourId;
            TouristId = dto.TouristId;
            Rating = dto.Rating;
            Comment = dto.Comment;
            TouristUsername = dto.TouristUsername;
        }

        public TourRate(long tourId, long touristId, int rating, string comment)
        {
            TourId = tourId;
            TouristId = touristId;
            Rating = rating;
            Comment = comment;
        }

    }
}

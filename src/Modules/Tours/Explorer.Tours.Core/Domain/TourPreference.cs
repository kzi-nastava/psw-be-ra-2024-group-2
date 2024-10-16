using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Reflection.Metadata;

namespace Explorer.Tours.Core.Domain
{
    public class TourPreference : Entity
    {
        public long TouristId { get; private set; }
        public DifficultyLevel Difficulty { get; private set; }
        public int WalkRating { get; private set; }
        public int BicycleRating { get; private set; }
        public int CarRating { get; private set; }
        public int BoatRating { get; private set; }
        public List<TourPreferenceTag>? Tags { get; private set; }

        public TourPreference() { }
        public TourPreference(long touristId, DifficultyLevel difficulty, int walkRating, int bicycleRating, int carRating, int boatRating, List<TourPreferenceTag>? tags)
        {
            // TODO: Move to validate
            if (!IsRatingValid(walkRating))
                throw new ArgumentOutOfRangeException(nameof(walkRating), "rating must be between 0 and 3.");
            if (!IsRatingValid(bicycleRating))
                throw new ArgumentOutOfRangeException(nameof(bicycleRating), "rating must be between 0 and 3.");
            if (!IsRatingValid(carRating))
                throw new ArgumentOutOfRangeException(nameof(carRating), "rating must be between 0 and 3.");
            if (!IsRatingValid(boatRating))
                throw new ArgumentOutOfRangeException(nameof(boatRating), "rating must be between 0 and 3.");

            TouristId = touristId;
            Difficulty = difficulty;
            WalkRating = walkRating;
            BicycleRating = bicycleRating;
            CarRating = carRating;
            BoatRating = boatRating;
            Tags = tags;

            Validate();
        }

        private void Validate()
        {
            // TODO: Implement validation
            throw new NotImplementedException();
        }

        private bool IsRatingValid(int rating)
        {
            return rating >= 0 && rating <= 3;
        }
    }

    public enum DifficultyLevel
    {
        Easy,
        Moderate,
        Hard
    }
}

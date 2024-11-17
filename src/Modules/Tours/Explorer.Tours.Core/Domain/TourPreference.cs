using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.Core.Domain;

public class TourPreference : Entity
{
    public long TouristId { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public int WalkRating { get; set; }
    public int BicycleRating { get; set; }
    public int CarRating { get; set; }
    public int BoatRating { get; set; }
    public List<TourPreferenceTag>? Tags { get; set; }

    public TourPreference() { }
    public TourPreference(long touristId, DifficultyLevel difficulty, int walkRating, int bicycleRating, int carRating, int boatRating, List<TourPreferenceTag>? tags)
    {
        Validate(walkRating, bicycleRating, carRating, boatRating, difficulty);
        TouristId = touristId;
        Difficulty = difficulty;
        WalkRating = walkRating;
        BicycleRating = bicycleRating;
        CarRating = carRating;
        BoatRating = boatRating;
        Tags = tags;
    }

    private void Validate(int walk, int bicycle, int car, int boat, DifficultyLevel difficultyLevel)
    {
        if (!IsRatingValid(walk))
            throw new ArgumentOutOfRangeException(nameof(walk), "rating must be between 0 and 3.");
        if (!IsRatingValid(bicycle))
            throw new ArgumentOutOfRangeException(nameof(bicycle), "rating must be between 0 and 3.");
        if (!IsRatingValid(car))
            throw new ArgumentOutOfRangeException(nameof(car), "rating must be between 0 and 3.");
        if (!IsRatingValid(boat))
            throw new ArgumentOutOfRangeException(nameof(boat), "rating must be between 0 and 3.");
        if (!IsDifficultyValid(difficultyLevel))
            throw new ArgumentOutOfRangeException(nameof(difficultyLevel), "Invalid difficulty level.");
    }

    private bool IsRatingValid(int rating)
    {
        return rating >= 0 && rating <= 3;
    }
    private bool IsDifficultyValid(DifficultyLevel difficulty)
    {
        return difficulty == DifficultyLevel.Easy ||
               difficulty == DifficultyLevel.Moderate ||
               difficulty == DifficultyLevel.Hard;
    }
}

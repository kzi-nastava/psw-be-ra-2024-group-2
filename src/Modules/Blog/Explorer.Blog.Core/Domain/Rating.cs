using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System.Text.Json.Serialization;

namespace Explorer.Blog.Core.Domain
{
    public class Rating : ValueObject
    {
        public RatingType RatingType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Username { get; private set; }

        [JsonConstructor]
        public Rating(RatingType ratingType, DateTime createdAt, string username)
        {
            RatingType = ratingType;
            CreatedAt = createdAt;
            Username = username;   
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return RatingType;
            yield return CreatedAt;
            yield return Username;
        }
    }
}

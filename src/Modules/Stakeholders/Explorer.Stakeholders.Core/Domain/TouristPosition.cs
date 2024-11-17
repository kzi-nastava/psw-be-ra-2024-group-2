using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System.Text.Json.Serialization;

namespace Explorer.Stakeholders.Core.Domain
{
    public class TouristPosition : ValueObject
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [JsonConstructor]
        public TouristPosition(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public TouristPosition() { }


        public void UpdatePosition(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Latitude; yield return Longitude;
        }
    }
}

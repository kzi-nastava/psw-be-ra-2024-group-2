using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class EventAcception : ValueObject
    {
        public long TouristId { get; private set; }
        public DateTime? AcceptedAt { get; private set; }


        public EventAcception(long touristId, DateTime? acceptedAt)
        {
            TouristId = touristId;
            AcceptedAt = acceptedAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TouristId;
            yield return AcceptedAt;
        }
    }
}

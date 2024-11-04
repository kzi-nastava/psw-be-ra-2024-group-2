using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourExecutionCheckpoint : ValueObject
    {
        public long CheckpointId {  get; private set; }
        public DateTime? ArrivalAt { get; private set; }

        [JsonConstructor]
        public TourExecutionCheckpoint(long checkpointId, DateTime? arrivalAt)
        {
            CheckpointId = checkpointId;
            ArrivalAt = arrivalAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CheckpointId;
            yield return ArrivalAt;
        }

    }
}

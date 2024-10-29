using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourExecutionCheckpoint : Entity
    {
        public long TourExecutionId {  get; private set; }
        public long CheckpointId {  get; private set; }
        public Checkpoint Checkpoint { get; private set; }
        public CheckpointStatus Status { get; private set; }
        public DateTime ArrivalAt {  get; private set; }


        public TourExecutionCheckpoint(long tourExecutionId, CheckpointStatus status, DateTime arrivalAt, long checkpointId)
        {
            TourExecutionId = tourExecutionId;
            Status = status;
            ArrivalAt = arrivalAt;
            CheckpointId = checkpointId;
        }



    }
}

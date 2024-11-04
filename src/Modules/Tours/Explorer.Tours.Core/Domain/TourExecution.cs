using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourExecution : Entity
    {
        public long UserId { get; private set; }
        public long TourId { get; private set; }
        public TourExecutionStatus Status { get; private set; }
        public DateTime? SessionEndingTime { get; private set; } = null; // na pocetku null, setovace se kad se predju svi cp ili se napusti tura
        public DateTime LastActivity { get; private set; }
        public List<TourExecutionCheckpoint> TourExecutionCheckpoints { get; private set; } = new List<TourExecutionCheckpoint> { };


        public TourExecution(long userId, long tourId, TourExecutionStatus status, DateTime lastActivity)
        {
            UserId = userId;
            TourId = tourId;
            Status = status;
            LastActivity = lastActivity;
        }

        public TourExecution() { }

        public double GetProgress()
        {
            return TourExecutionCheckpoints.Where(c => c.ArrivalAt != null).Count() / TourExecutionCheckpoints.Count();
        }

        public bool IsLastActivityOlderThanSevenDays()
        {
            return (DateTime.UtcNow - LastActivity).TotalDays > 7;
        }
    }
}

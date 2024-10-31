using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public class TourExecution : Entity
    {
        public long UserId { get; private set; }
        public long TourId {  get; private set; }
        public Tour Tour { get; private set; }
        public TourExecutionStatus Status { get; private set; }
        public DateTime? SessionEndingTime {  get; private set; }
        public DateTime LastActivity {  get; private set; }

        public List<TourExecutionCheckpoint> tourExecutionCheckpoints { get; private set; } = new List<TourExecutionCheckpoint> { };


        public TourExecution(long userId, long tourId, TourExecutionStatus status, DateTime sessionEndingTime, DateTime lastActivity)
        {
            UserId = userId;
            TourId = tourId;
            Status = status;
            SessionEndingTime = sessionEndingTime;
            LastActivity = lastActivity;
        }

        public TourExecution() { }

        public double GetProgress()
        {
            return tourExecutionCheckpoints.Where(c => c.Status == CheckpointStatus.Completed).Count() / tourExecutionCheckpoints.Count();
        }

        public bool IsLastActivityOlderThanSevenDays()
        {
            return (DateTime.UtcNow - LastActivity).TotalDays > 7;
        }
    }
}

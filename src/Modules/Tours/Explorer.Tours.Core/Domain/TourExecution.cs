using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.Core.Domain;

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
        return TourExecutionCheckpoints.Where(c => c.ArrivalAt != null).Count() / (double)TourExecutionCheckpoints.Count();
    }

    public bool IsLastActivityOlderThanSevenDays()
    {
        return (DateTime.UtcNow - LastActivity).TotalDays > 7;
    }

    internal void UpdateCheckpoints(List<TourExecutionCheckpoint> execCheckpoints)
    {
        TourExecutionCheckpoints.Clear();
        TourExecutionCheckpoints.AddRange(execCheckpoints);
    }

    internal void UpdateTourActivity(DateTime lastActivity)
    {
        LastActivity = lastActivity;
    }

    public bool CheckCheckpoints(List<TourExecutionCheckpoint> checkpoints)
    {
        foreach(TourExecutionCheckpoint checkpoint in checkpoints)
        {
            if (checkpoint.ArrivalAt != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }


    internal void ChangeEndStatusAndEndingTime(bool result)
    {
        if(result)
        {
            Status = TourExecutionStatus.Completed;
        }
        else
        {
            Status = TourExecutionStatus.Abandoned;
        }

        SessionEndingTime = DateTime.UtcNow;
    }
}

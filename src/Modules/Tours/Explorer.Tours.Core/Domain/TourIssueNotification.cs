using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.Core.Domain
{
    public class TourIssueNotification: Entity
    {
        public long FromUserId { get;private set; }
        public long ToUserId { get; private set; }
        public TourIssueNotificationStatus Status { get; private set; }
        public long TourIssueReportId { get; private set; }
        public TourIssueReport TourIssueReport { get; private set; }
        public TourIssueNotification() { }

        public TourIssueNotification(long fromUserId, long toUserId, TourIssueNotificationStatus status, long tourIssueReportId)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Status = status;
            TourIssueReportId = tourIssueReportId;
        }
        public void UpdateStatus(TourIssueNotificationStatus status)
        {
            Status = status;
        }
        public void Read()
        {
            Status = TourIssueNotificationStatus.Read;
        }
        public void UnRead()
        {
            Status = TourIssueNotificationStatus.Unread;
        }
    }
}
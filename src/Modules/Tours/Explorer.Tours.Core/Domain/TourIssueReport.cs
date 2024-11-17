using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.Core.Domain
{
    public class TourIssueReport : Entity
    {
        public string Category { get; private set; }
        public string Description { get; private set; }
        public string Priority { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime FixUntil { get; private set; }
        public long UserId { get; private set; }
        public long TourId { get; private set; }
        public TourIssueReportStatus Status { get; private set; }
        public List<TourIssueComment>? TourIssueComments { get; private set; } = new List<TourIssueComment>();
        public List<TourIssueNotification>? TourIssueNotifications { get; private set; } = new List<TourIssueNotification>();
        public TourIssueReport(){}
        public TourIssueReport(string category, string description, string priority, DateTime createdAt, DateTime fixUntil, long userId, long tourId)
        {
            Category = category;
            Description = description;
            Priority = priority;
            CreatedAt = createdAt;
            FixUntil = fixUntil;
            UserId = userId;
            TourId = tourId;
            Validate();
        }
        
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Category)) throw new ArgumentException("Invalid Category.");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description.");
            if (string.IsNullOrWhiteSpace(Priority)) throw new ArgumentException("Invalid Priority.");

        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateCategory(string category)
        {
            Category = category;
        }

        public void UpdatePriority(string priority)
        {
            Priority = priority;
        }

        public void UpdateFixUntil(DateTime dateLimit)
        {
            FixUntil = dateLimit;
        }
        public void UpdateStatus(TourIssueReportStatus status)
        {
            Status = status;
        }
        public void CloseReport()
        {
            this.Status = TourIssueReportStatus.Closed;
        }
        public void MarkAsDone()
        {
            if (this.Status != TourIssueReportStatus.Open)
                throw new InvalidOperationException("Only open reports can be marked as done.");

            this.Status = TourIssueReportStatus.Closed;
        }

        public void SetFixUntil(DateTime date)
        {
            if (date <= DateTime.UtcNow)
                throw new ArgumentException("Fix until date must be in the future.");

            this.FixUntil = date;
        }
        public void AlertNotDone()
        {
            if (Status != TourIssueReportStatus.Open)
                throw new InvalidOperationException("Only open reports can be marked as not done.");
            FixUntil = DateTime.UtcNow;
        }
        public bool ReadNotification(long notificationId)
        {
            TourIssueNotification notification = TourIssueNotifications.FirstOrDefault(t => t.Id == notificationId);
            if(notification != null)
            {
                notification.Read();
                return true;
            }
            return false;
        }
        public void UnReadNotification(long notificationId)
        {
            TourIssueNotification notification = TourIssueNotifications.FirstOrDefault(t => t.Id == notificationId);
            if(notification != null)
            {
                notification.UnRead();
            }
        }
    }
}
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourIssueNotification: Entity
    {
        public long UserId { get;private set; }
        public TourIssueNotificationStatus Status { get; private set; }
        public long TourIssueReportId { get; private set; }
        public TourIssueReport TourIssueReport { get; private set; }
        public TourIssueNotification() { }

        public TourIssueNotification(long userId, TourIssueNotificationStatus status, long tourIssueReportId)
        {
            UserId = userId;
            Status = status;
            TourIssueReportId = tourIssueReportId;
        }
        public void UpdateStatus(TourIssueNotificationStatus status) 
        {
            Status = status;
        }
    }
}
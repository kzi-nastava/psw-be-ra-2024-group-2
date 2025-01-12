using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourIssueNotificationDto
    {
        public long Id { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public TourIssueNotificationStatus Status { get; set; }
        public long TourIssueReportId { get; set; }
        public string? fromUsername { get; set; }
        public string?toUsername { get; set; }
    }
}
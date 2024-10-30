using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourIssueComment : Entity
    {
        public long UserId { get; private set; }
        public long TourIssueReportId { get; private set; }
        public string Comment { get; private set; }
        public DateTime PublishedAt { get; private set; }
        public TourIssueReport TourIssueReport { get; private set; }
        public TourIssueComment(){}
        public TourIssueComment(long userId, long tourIssueReportId, string comment, DateTime publishedAt)
        {
            UserId = userId;
            TourIssueReportId = tourIssueReportId;
            Comment = comment;
            PublishedAt = publishedAt;
        }
        public void UpdateComment(string comment)
        {
            Comment = comment;
        }
    }
}
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public Tour Tour { get; private set; }
        public TourIssueReportStatus Status { get; private set; }
        public List<TourIssueComment>? TourIssueComments { get; private set; } = new List<TourIssueComment>();
        public TourIssueReport(){}
        public TourIssueReport(string category, string description, string priority, DateTime createdAt, DateTime fixUntil, long userId, long tourId)
        {
            Category = category;
            Description = description;
            Priority = priority;
            CreatedAt = createdAt;
            FixUntil = createdAt.AddYears(500);
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
    }
}

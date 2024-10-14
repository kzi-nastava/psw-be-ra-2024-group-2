using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
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

        public DateTime DateTime { get; private set; }

        public long UserId { get; private set; }

        public long TourId { get; private set; }
        public TourIssueReport(string category, string description, string priority, DateTime dateTime, long userId, long tourId)
        {

            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Invalid Category.");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
            if (string.IsNullOrWhiteSpace(priority)) throw new ArgumentException("Invalid Priority.");

            if (userId < 0) throw new ArgumentException("UserID cannot be negative.");
            if (tourId < 0) throw new ArgumentException("TourID cannot be negative.");

            Category = category;
            Description = description;
            Priority = priority;
            DateTime = dateTime;
            UserId = userId;
            TourId = tourId;
        }
    }
}

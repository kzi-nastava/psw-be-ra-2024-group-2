using System;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class TourProblemReport : Entity
    {
        // Constructor (if necessary)
        public TourProblemReport()
        {
        }
        public TourProblemReport(long tourId, long touristId, long guideId, string name, string description, string status)
        {
            TourId = tourId;
            TouristId = touristId;
            GuideId = guideId;
            Name = name;
            Description = description;
            Status = status;
        }
        // Fields corresponding to the table columns
        public long TourId { get; private set; } // Foreign key to Tours
        public long TouristId { get; private set; } // Foreign key to Tourists
        public long GuideId { get; private set; } // Foreign key to Guides
        public string Name { get; private set; } // Name of the report
        public string Description { get; private set; } // Description of the problem
        public string Status { get; private set; } // Status of the report
    }
}

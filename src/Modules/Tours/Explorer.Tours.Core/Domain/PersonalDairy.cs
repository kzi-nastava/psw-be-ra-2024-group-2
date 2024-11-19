using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Tours.Core.Domain
{
    public class PersonalDairy : Entity
    {
        public long UserId { get; private set; }
        public long TourId { get; private set; }
        public string Title { get; private set; }        
        public DateTime CreatedAt { get; private set; }
        public DairyStatus Status { get; private set; }

        // public List<Chapter> Chapters { get; private set; } = new List<Chapter> { };


        public PersonalDairy() { }
    public PersonalDairy(long userId, long tourId, string title) { 

        UserId = userId;
        TourId = tourId;
        Title = title;
        CreatedAt = DateTime.UtcNow;
        Status = DairyStatus.InProgress;
    }



    }
}

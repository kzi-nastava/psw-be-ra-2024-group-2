using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Tours.Core.Domain
{
    public class TourInvite : Entity
    {
        public long OwnerId { get; set; }
        public long TouristId { get; set; }
        public DateTime Date { get; set; }
        public TourInviteStatus Status { get; set; }
        public TourInvite(long ownerId,long touristId,DateTime date,TourInviteStatus status) {
            OwnerId = ownerId;
            TouristId = touristId;
            Date = date;
            Status = status;
            Validate();
        }

        private void Validate()
        {
            if (OwnerId < 0) throw new ArgumentException("Has to have a club owner");
            if (TouristId < 0) throw new ArgumentException("Has to have a tourist to invite");
            if (Date.Year < 1823) throw new ArgumentException("Computers weren't even made that year");
            if (Status == TourInviteStatus.Accepted || Status ==  TourInviteStatus.Rejected || Status == TourInviteStatus.Pending ) throw new ArgumentException("Status is not valid");
        }
    }
    public enum TourInviteStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
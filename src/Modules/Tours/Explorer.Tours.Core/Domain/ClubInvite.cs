using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Tours.Core.Domain
{
    public class ClubInvite : Entity
    {
        public long OwnerId { get; set; }
        public long TouristId { get; set; }
        public long ClubId { get; set; }
        public DateTime Date { get; set; }
        public TourInviteStatus Status { get; set; }
        public ClubInvite(long ownerId,long touristId,DateTime date,TourInviteStatus status) {
            OwnerId = ownerId;
            TouristId = touristId;
            Date = date;
            Status = status;
            Validate();
        }

        private void Validate()
        {
            if (Date.Year < 1823) throw new ArgumentException("Computers weren't even made that year"); 
            if (!Enum.IsDefined(typeof(TourInviteStatus), Status)) throw new ArgumentException("Status is not valid");

        }
    }
    public enum TourInviteStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
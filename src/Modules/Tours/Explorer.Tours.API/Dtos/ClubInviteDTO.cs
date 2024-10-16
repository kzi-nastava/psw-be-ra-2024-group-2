using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class ClubInviteDTO
    {
        public long OwnerId { get; set; }
        public long TouristId { get; set; }
        public long ClubId { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public TourInviteStatus Status { get; set; }
    }
    public enum TourInviteStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
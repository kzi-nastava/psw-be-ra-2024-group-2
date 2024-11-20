using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class EventAcceptionDto
    {
        public int Id { get; set; }
        public long TouristId { get; set; }
        public DateTime? AcceptedAt { get; set; }
    }
}

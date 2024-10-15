using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourIssueReportDto
    {
        //public long Id { get; set; }
        public string Category { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        public DateTime DateTime { get; set; }

        public long TourId { get; set; }
        public long UserId { get; set; }
    }
}

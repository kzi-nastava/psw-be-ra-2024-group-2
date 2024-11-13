using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class MarkAsReadDto
    {
        public long UserId { get; set; }
        public long TourIssueReportId { get; set; }
    }
}

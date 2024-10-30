﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourIssueCommentDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TourIssueReportId { get; set; }
        public string Comment { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}

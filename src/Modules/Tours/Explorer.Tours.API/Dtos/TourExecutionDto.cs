using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourExecutionDto
    {
        public int Id { get; set; }

        public long UserId { get;  set; }
        public long TourId { get;  set; }
        public TourDto Tour { get;  set; }
        public TourExecutionStatus Status { get;  set; }
        public DateTime SessionEndingTime { get;  set; }
        public DateTime LastActivity { get;  set; }

        public List<TourExecutionCheckpointDto> tourExecutionCheckpoints { get;  set; } = new List<TourExecutionCheckpointDto>{ };
    }
}
